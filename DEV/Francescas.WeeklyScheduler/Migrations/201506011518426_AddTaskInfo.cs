namespace Francescas.WeeklyScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTaskInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WeeklyScheduleCsv", "TASK", c => c.String());
            AddColumn("dbo.WeeklyScheduleCsv", "HOURS", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WeeklyScheduleCsv", "HOURS");
            DropColumn("dbo.WeeklyScheduleCsv", "TASK");
        }
    }
}
