namespace Francescas.WeeklyScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableOvr : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WeeklyScheduleCsv", "OVR", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WeeklyScheduleCsv", "OVR", c => c.Double(nullable: false));
        }
    }
}
