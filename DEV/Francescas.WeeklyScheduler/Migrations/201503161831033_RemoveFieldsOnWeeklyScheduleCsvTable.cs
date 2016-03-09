namespace Francescas.WeeklyScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFieldsOnWeeklyScheduleCsvTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.WeeklyScheduleCsv", "ModifiedDateTime");
            DropColumn("dbo.WeeklyScheduleCsv", "CreatedDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WeeklyScheduleCsv", "CreatedDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.WeeklyScheduleCsv", "ModifiedDateTime", c => c.DateTime(nullable: false));
        }
    }
}
