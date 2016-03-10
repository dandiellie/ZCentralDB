namespace ZCentralDB.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableDetailsForProducts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "OrganizationId", "dbo.Organizations");
            DropIndex("dbo.Products", new[] { "OrganizationId" });
            AlterColumn("dbo.Products", "Weight", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Products", "OrganizationId", c => c.Int());
            AlterColumn("dbo.Details", "Price", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Details", "OldPrice", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("dbo.Products", "OrganizationId");
            AddForeignKey("dbo.Products", "OrganizationId", "dbo.Organizations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "OrganizationId", "dbo.Organizations");
            DropIndex("dbo.Products", new[] { "OrganizationId" });
            AlterColumn("dbo.Details", "OldPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Details", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Products", "OrganizationId", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.Products", "OrganizationId");
            AddForeignKey("dbo.Products", "OrganizationId", "dbo.Organizations", "Id", cascadeDelete: true);
        }
    }
}
