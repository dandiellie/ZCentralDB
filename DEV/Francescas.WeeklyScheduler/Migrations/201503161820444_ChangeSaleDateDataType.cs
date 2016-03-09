namespace Francescas.WeeklyScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSaleDateDataType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WeeklyScheduleCsv", "SALEDATE", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WeeklyScheduleCsv", "SALEDATE", c => c.DateTime(nullable: false));
        }
    }
}
