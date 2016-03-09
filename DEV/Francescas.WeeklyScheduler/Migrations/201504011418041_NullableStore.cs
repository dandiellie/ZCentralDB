namespace Francescas.WeeklyScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableStore : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WeeklyScheduleCsv", "STORE", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WeeklyScheduleCsv", "STORE", c => c.Int(nullable: false));
        }
    }
}
