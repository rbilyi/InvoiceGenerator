namespace Nakladna.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpecialPrice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Return = c.Int(nullable: false),
                        Producer = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Customer_Id = c.Int(),
                        GoodType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.GoodTypes", t => t.GoodType_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.GoodType_Id);
            
            CreateTable(
                "dbo.GoodTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Double(nullable: false),
                        ColumnInDocument = c.Int(nullable: false),
                        HasReturn = c.Boolean(nullable: false),
                        ReturnColumn = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SpecialPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Customer_Id = c.Int(),
                        GoodType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.GoodTypes", t => t.GoodType_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.GoodType_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpecialPrices", "GoodType_Id", "dbo.GoodTypes");
            DropForeignKey("dbo.SpecialPrices", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Sales", "GoodType_Id", "dbo.GoodTypes");
            DropForeignKey("dbo.Sales", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.SpecialPrices", new[] { "GoodType_Id" });
            DropIndex("dbo.SpecialPrices", new[] { "Customer_Id" });
            DropIndex("dbo.Sales", new[] { "GoodType_Id" });
            DropIndex("dbo.Sales", new[] { "Customer_Id" });
            DropTable("dbo.SpecialPrices");
            DropTable("dbo.GoodTypes");
            DropTable("dbo.Sales");
            DropTable("dbo.Customers");
        }
    }
}
