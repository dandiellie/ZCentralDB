namespace Francescas.WeeklyScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Store",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreNumber = c.Int(nullable: false),
                        StoreEmail = c.String(),
                        ModifiedDateTime = c.DateTime(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WeeklyScheduleCsv",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Store = c.Int(nullable: false),
                        SaleDate = c.DateTime(nullable: false),
                        Ovr = c.Double(nullable: false),
                        SentDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WeeklyScheduleCsv");
            DropTable("dbo.Store");
        }
    }
}
