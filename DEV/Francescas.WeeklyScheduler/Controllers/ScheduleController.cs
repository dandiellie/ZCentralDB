using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Francescas.WeeklyScheduler.Mailers;
using Francescas.WeeklyScheduler.Models;
using Microsoft.Ajax.Utilities;
using Mvc.Mailer;
using Excel = NetOffice.ExcelApi;
using System.Threading.Tasks;


namespace Francescas.WeeklyScheduler.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly FrancescasContext _context = new FrancescasContext();
        private IScheduleMailer _scheduleMailer = new ScheduleMailer();

        public IScheduleMailer ScheduleMailer
        {
            get { return _scheduleMailer; }
            set { _scheduleMailer = value; }
        }

        // GET: Schedule
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadInputFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            try
            {
            
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var path = Server.MapPath(ConfigurationManager.AppSettings["sourceSalesPlanningCsvFile"]);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    file.SaveAs(path);
                }
                else
                {
                    throw new Exception("A file upload was not attempted.");
                }
            }
            else
            {
                throw new Exception("A file upload was not attempted.");
            }
            return RedirectToAction("ProcessCsv");
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new List<string> { ex.Message };
                
            }

            return View("UploadInputFile");
        }

        public ActionResult ProcessCsv()
        {
            var records = _context.WeeklyScheduleCsv.ToList();
            _context.WeeklyScheduleCsv.RemoveRange(records);
            _context.SaveChanges();

            var sourceFilePath = Server.MapPath(ConfigurationManager.AppSettings["sourceSalesPlanningCsvFile"]);

            var dt = ManageCsv.GetDataTableFromCsvFile(sourceFilePath);
            ManageCsv.InsertDataIntoSqlServer(dt);


            return View();
        }

        public ActionResult EmailSchedule(int id)
        {
            var viewModel = new EmailScheduleViewModel
            {
                Store = id,
                Date = DateTime.Now.Date.AddDays(14)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> EmailSchedule(EmailScheduleViewModel _viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sourceFile = Server.MapPath(ConfigurationManager.AppSettings["sourceScheduleExcelFile"]);
                    var destFilePath =
                        Server.MapPath(ConfigurationManager.AppSettings["scheduleDestinationDirectory"] +
                                       _viewModel.Store +
                                       ".xlsx");
                    ScheduleHelper.GenerateSchedule(sourceFile, _viewModel.Store, _viewModel.Date, _context,
                        destFilePath);

                    var isCustomEmail = (!string.IsNullOrEmpty(_viewModel.CustomEmail) ||
                                         !string.IsNullOrWhiteSpace(_viewModel.CustomEmail));


                    var model = new ScheduleMailerSend
                    {
                        StoreNumber = _viewModel.Store,
                        StoreEmail =
                            isCustomEmail
                                ? _viewModel.CustomEmail
                                : "fran" + _viewModel.Store.ToString("D3") + "a@francescas.com",
                        FilePath = destFilePath
                    };


                    var message = ScheduleMailer.Schedule(model);

                    var client = new SmtpClientWrapper();
                    client.SendCompleted += (sender, e) =>
                    {
                        if (message != null)
                        {
                            message.Attachments.Dispose();
                            message.Dispose();

                            if (System.IO.File.Exists(destFilePath))
                            {
                                System.IO.File.Delete(destFilePath);
                            }
                        }

                        if (e.Error != null || e.Cancelled)
                        {
                            //Do something with the error?
                        }
                        else
                        {
                            _context.WeeklyScheduleCsv
                                .Where(p => p.STORE == model.StoreNumber)
                                .ToList()
                                .ForEach(a => a.SentDateTime = DateTime.Now);
                            _context.SaveChanges();
                        }

                        client.Dispose();
                    };

                    await message.SendAsync("schedule message", client);

                    return RedirectToAction("ThankYou");
                }
            }
            catch (Exception e)
            {
                return View("Error");
            }
            return View("Error");
        }


        public ActionResult EmailAllSchedules()
        {
            var vm = new EmailAllSchedulesViewModel
            {
                Date = DateTime.Now.Date.AddDays(14)
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> EmailAllSchedules(EmailAllSchedulesViewModel _viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<EmailSchedulesModel> emails = new List<EmailSchedulesModel>();

                    var isCustomEmail = (!string.IsNullOrEmpty(_viewModel.CustomEmail) ||
                                         !string.IsNullOrWhiteSpace(_viewModel.CustomEmail));

                    var nextSunday = _viewModel.Date.AddDays(7 - (int) _viewModel.Date.DayOfWeek);
                    var prevSunday = nextSunday.AddDays(-7);

                    var storeNums =
                        _viewModel.Stores.Where(p => p.IsChecked).Select(p => p.StoreNumber).Distinct().ToList();

                    foreach (var storeNum in storeNums)
                    {
                        var num = storeNum;

                        var sourceFile = Server.MapPath(ConfigurationManager.AppSettings["sourceScheduleExcelFile"]);
                        var destFilePath =
                            Server.MapPath(ConfigurationManager.AppSettings["scheduleDestinationDirectory"] + num +
                                           ".xlsx");
                        ScheduleHelper.GenerateSchedule(sourceFile, num, _viewModel.Date, _context, destFilePath);

                        emails.Add(new EmailSchedulesModel
                        {
                            FileGeneratDateTime = DateTime.Now,
                            FilePath = destFilePath,
                            StoreNumber = num
                        });

                        ScheduleHub.MassEmailStatus(storeNum, "Generating Schedules...", storeNums.Count*2, emails.Count);
                    }

                    emails = emails.OrderBy(p => p.FileGeneratDateTime).ToList();
                    var emailsSent = 0;
                    foreach (var email in emails)
                    {
                        var model = new ScheduleMailerSend
                        {
                            StoreNumber = email.StoreNumber,
                            StoreEmail =
                                isCustomEmail
                                    ? _viewModel.CustomEmail
                                    : "fran" + email.StoreNumber.ToString("D3") + "a@francescas.com",
                            FilePath = email.FilePath,
                            FileBytes = System.IO.File.ReadAllBytes(email.FilePath)
                        };

                        var message = ScheduleMailer.AnotherSchedule(model);
                        var fp = email.FilePath;
                        await Task.Factory
                            .StartNew(() =>
                            {
                                message.Send();

                            })
                            .ContinueWith((prevTask) =>
                            {
                                emailsSent++;
                                ScheduleHub.MassEmailStatus(email.StoreNumber, "E-Mailing Schedules...", storeNums.Count*2, emailsSent + emails.Count);
                                if (message != null)
                                {
                                    message.Attachments.Dispose();
                                    message.Dispose();

                                    if (System.IO.File.Exists(fp))
                                    {
                                        System.IO.File.Delete(fp);
                                    }
                                }

                                using (FrancescasContext con = new FrancescasContext())
                                {
                                    con.WeeklyScheduleCsv
                                        .Where(p => p.STORE == model.StoreNumber).ToList()
                                        .ForEach(a => { a.SentDateTime = DateTime.Now; });
                                    con.SaveChanges();
                                }
                            }, TaskContinuationOptions.NotOnFaulted);
                    }

                    //_context.SaveChanges();
                    return RedirectToAction("ThankYou");
                }
                else
                {
                    ViewBag.Errors = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage);
                }
                //return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new List<string> {ex.Message};
            }

            return View(_viewModel);
        }

        public ActionResult ThankYou()
        {
            return View();
        }

        public ActionResult DownloadSchedule(int id)
        {
            var viewModel = new DownloadScheduleViewModel
            {
                Store = id,
                Date = DateTime.Now.Date.AddDays(14)
            };

            return View(viewModel);
        }

        public static bool IsFileReady(String sFilename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(sFilename);
                if (fileBytes.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                
            }
            catch (Exception)
            {
                return false;
            }
        }


        [HttpPost]
        public ActionResult DownloadSchedule(DownloadScheduleViewModel _viewModel)
        {
            try
            {
                var sourceFile = Server.MapPath(ConfigurationManager.AppSettings["sourceScheduleExcelFile"]);
                var destFilePath =
                    Server.MapPath(ConfigurationManager.AppSettings["scheduleDestinationDirectory"] + _viewModel.Store +
                                   ".xlsx");
                ScheduleHelper.GenerateSchedule(sourceFile, _viewModel.Store, _viewModel.Date, _context, destFilePath);
                ScheduleHub.MassEmailStatus(_viewModel.Store, "some message", 1, 1);
                //byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("/ScheduleResources/schedule.xlsx"));
                var fileReady = false;
                while (!fileReady)
                {
                    fileReady = IsFileReady(destFilePath);
                }
                byte[] fileBytes = System.IO.File.ReadAllBytes(destFilePath);
                string fileName = _viewModel.Store.ToString() + ".xlsx";
                if (System.IO.File.Exists(destFilePath))
                {
                    System.IO.File.Delete(destFilePath);
                }
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new List<string> {ex.Message};
            }
            return View(_viewModel);
        }

        public ActionResult ManageStores()
        {
            var stores = _context.WeeklyScheduleCsv.DistinctBy(p => p.STORE);

            var vm = new ManageStoresViewModel
            {
                Stores = _context.WeeklyScheduleCsv
                    .Where(p => p.STORE != null)
                    .Select(
                        p =>
                            new ManageStore
                            {
                                StoreNumber = (int) p.STORE,
                                SentDateTime = p.SentDateTime,
                                IsChecked = false
                            })
                    .DistinctBy(p => p.StoreNumber).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult ManageStores([FromJson] ManageStoresViewModel _viewModel)
        {
            var vm = new EmailAllSchedulesViewModel
            {
                Date = DateTime.Now.Date.AddDays(14),
                Stores = _viewModel.Stores
            };

            return View("EmailAllSchedules", vm);
        }

        #region edit

        //public ActionResult EditDate(int id)
        //{
        //    var vm = new EditScheduleViewModel
        //    {
        //        Date = DateTime.Now.Date,
        //        Store = id
        //    };

        //    return View(vm);
        //}

        //[HttpPost]
        //public ActionResult EditDate(EditScheduleViewModel _viewModel)
        //{
        //    return RedirectToAction("Edit", _viewModel);
        //}

        //public ActionResult Edit(EditScheduleViewModel _viewModel)
        //{
        //    var nextSunday = _viewModel.Date.AddDays(7 - (int)_viewModel.Date.DayOfWeek);
        //    var prevSunday = nextSunday.Date.AddDays(-7);

        //    var viewModel = new EditViewModel
        //    {
        //        Store = _viewModel.Store,
        //        Records = (_context.WeeklyScheduleCsv
        //            .Where(r => r.STORE == _viewModel.Store))
        //            .AsEnumerable()
        //            .Select(r => new EditRecord
        //            {
        //                Ovr = r.OVR,
        //                SaleDate = DateTime.ParseExact(r.SALEDATE, "yyyyMMdd", CultureInfo.InvariantCulture)
        //            })
        //            .Where(r => r.SaleDate.Date < nextSunday.Date && r.SaleDate.Date >= prevSunday)
        //            .OrderBy(r=>r.SaleDate)
        //    };
        //    return View(viewModel);
        //}

        //[HttpPost]
        //public ActionResult Edit(EditViewModel _viewModel)
        //{
        //    foreach (var item in _viewModel.Records)
        //    {
        //        var record = new WeeklyScheduleCsv
        //        {
        //            STORE = _viewModel.Store,
        //            OVR = item.Ovr,
        //            SALEDATE = item.SaleDate.ToString("yyyyMMdd")
        //        };

        //        _context.WeeklyScheduleCsv.AddOrUpdate(p => new {p.STORE, p.SALEDATE}, record);
        //    }

        //    _context.SaveChanges();

        //    return RedirectToAction("ManageStores");
        //}

        #endregion
    }

    public class ScheduleHelper
    {
        public static void GenerateSchedule(string sourceFile, int storeNumber, DateTime date, FrancescasContext context,
            string destinationFile = "")
        {
            Excel.Application excelApplication = new Excel.Application();
            excelApplication.DisplayAlerts = false;

            Excel.Workbook workBook = excelApplication.Workbooks.Open(sourceFile);
            Excel.Worksheet workSheet = (Excel.Worksheet) workBook.Worksheets[1];

            var nextSunday = date.Date.AddDays(7 - (int) date.Date.DayOfWeek);
            var prevSunday = nextSunday.AddDays(-7);

            var storeRecords = (context.WeeklyScheduleCsv
                .Where(r => r.STORE == storeNumber))
                .AsEnumerable()
                .Select(r => new
                {
                    Ovr = r.OVR,
                    SaleDate = DateTime.ParseExact(r.SALEDATE, "yyyyMMdd", CultureInfo.InvariantCulture),
                    TaskName = r.TASK,
                    TaskHours = r.HOURS
                })
                .Where(r => r.SaleDate.Date < nextSunday.Date && r.SaleDate.Date >= prevSunday)
                .OrderBy(r => r.SaleDate);
            workSheet.Unprotect(ConfigurationManager.AppSettings["scheduleSheetPassword"]);

            //Include Boutique number in title
            workSheet.Cells[3, 1].Value = "BOUTIQUE " + storeNumber + " SCHEDULE";

            //Saturday (last day of the week)
            workSheet.Cells[5, 2].Value = nextSunday.Date.AddDays(-1);

            //Display plan amount for corresponding dates
            workSheet.Cells[7, 2].Value =
                storeRecords.Where(r => r.SaleDate.Date == nextSunday.Date.AddDays(-7))
                    .Select(r => r.Ovr)
                    .FirstOrDefault();
            workSheet.Cells[7, 7].Value =
                storeRecords.Where(r => r.SaleDate.Date == nextSunday.Date.AddDays(-6))
                    .Select(r => r.Ovr)
                    .FirstOrDefault();
            workSheet.Cells[7, 12].Value =
                storeRecords.Where(r => r.SaleDate.Date == nextSunday.Date.AddDays(-5))
                    .Select(r => r.Ovr)
                    .FirstOrDefault();
            workSheet.Cells[7, 17].Value =
                storeRecords.Where(r => r.SaleDate.Date == nextSunday.Date.AddDays(-4))
                    .Select(r => r.Ovr)
                    .FirstOrDefault();
            workSheet.Cells[7, 22].Value =
                storeRecords.Where(r => r.SaleDate.Date == nextSunday.Date.AddDays(-3))
                    .Select(r => r.Ovr)
                    .FirstOrDefault();
            workSheet.Cells[7, 27].Value =
                storeRecords.Where(r => r.SaleDate.Date == nextSunday.Date.AddDays(-2))
                    .Select(r => r.Ovr)
                    .FirstOrDefault();
            workSheet.Cells[7, 32].Value =
                storeRecords.Where(r => r.SaleDate.Date == nextSunday.Date.AddDays(-1))
                    .Select(r => r.Ovr)
                    .FirstOrDefault();

            //Display correct day of week for each column
            workSheet.Cells[8, 2].Value = nextSunday.Date.AddDays(-7).ToString("dddd");
            workSheet.Cells[8, 7].Value = nextSunday.Date.AddDays(-6).ToString("dddd");
            workSheet.Cells[8, 12].Value = nextSunday.Date.AddDays(-5).ToString("dddd");
            workSheet.Cells[8, 17].Value = nextSunday.Date.AddDays(-4).ToString("dddd");
            workSheet.Cells[8, 22].Value = nextSunday.Date.AddDays(-3).ToString("dddd");
            workSheet.Cells[8, 27].Value = nextSunday.Date.AddDays(-2).ToString("dddd");
            workSheet.Cells[8, 32].Value = nextSunday.Date.AddDays(-1).ToString("dddd");

            //Display correct dates for each column
            workSheet.Cells[9, 2].Value = nextSunday.Date.AddDays(-7);
            workSheet.Cells[9, 7].Value = nextSunday.Date.AddDays(-6);
            workSheet.Cells[9, 12].Value = nextSunday.Date.AddDays(-5);
            workSheet.Cells[9, 17].Value = nextSunday.Date.AddDays(-4);
            workSheet.Cells[9, 22].Value = nextSunday.Date.AddDays(-3);
            workSheet.Cells[9, 27].Value = nextSunday.Date.AddDays(-2);
            workSheet.Cells[9, 32].Value = nextSunday.Date.AddDays(-1);

            //Display correct tasks for each date
            workSheet.Cells[10, 2].Value =
                storeRecords.Where(r => r.SaleDate.Date == nextSunday.Date.AddDays(-7))
                    .Select(r => (r.TaskName == null && r.TaskHours == null) ? "" : r.TaskName + " / " + r.TaskHours)
                    .FirstOrDefault();
            workSheet.Cells[10, 7].Value =
                storeRecords.Where(r => r.SaleDate.Date == nextSunday.Date.AddDays(-6))
                    .Select(r => (r.TaskName == null && r.TaskHours == null) ? "" : r.TaskName + " / " + r.TaskHours)
                    .FirstOrDefault();
            workSheet.Cells[10, 12].Value =
                storeRecords.Where(r => r.SaleDate.Date == nextSunday.Date.AddDays(-5))
                    .Select(r => (r.TaskName == null && r.TaskHours == null) ? "" : r.TaskName + " / " + r.TaskHours)
                    .FirstOrDefault();
            workSheet.Cells[10, 17].Value =
                storeRecords.Where(r => r.SaleDate.Date == nextSunday.Date.AddDays(-4))
                    .Select(r => (r.TaskName == null && r.TaskHours == null) ? "" : r.TaskName + " / " + r.TaskHours)
                    .FirstOrDefault();
            workSheet.Cells[10, 22].Value =
                storeRecords.Where(r => r.SaleDate.Date == nextSunday.Date.AddDays(-3))
                    .Select(r => (r.TaskName == null && r.TaskHours == null) ? "" : r.TaskName + " / " + r.TaskHours)
                    .FirstOrDefault();
            workSheet.Cells[10, 27].Value =
                storeRecords.Where(r => r.SaleDate.Date == nextSunday.Date.AddDays(-2))
                    .Select(r => (r.TaskName == null && r.TaskHours == null) ? "" : r.TaskName + " / " + r.TaskHours)
                    .FirstOrDefault();
            workSheet.Cells[10, 32].Value =
                storeRecords.Where(r => r.SaleDate.Date == nextSunday.Date.AddDays(-1))
                    .Select(r => (r.TaskName == null && r.TaskHours == null) ? "" : r.TaskName + " / " + r.TaskHours)
                    .FirstOrDefault();

            workSheet.Protect(ConfigurationManager.AppSettings["scheduleSheetPassword"]);
            if (destinationFile.IsNullOrWhiteSpace())
            {
                workBook.Save();
            }
            else
            {
                if (File.Exists(destinationFile))
                {
                    File.Delete(destinationFile);
                }

                workBook.SaveAs(destinationFile);
            }

            excelApplication.Quit();
            excelApplication.Dispose();
        }
    }

    public class EmailSchedulesModel
    {
        public int StoreNumber { get; set; }
        public string FilePath { get; set; }
        public DateTime FileGeneratDateTime { get; set; }
    }
}