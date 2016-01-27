namespace Nakladna.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class navigation_ids : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sales", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.SpecialPrices", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Sales", "GoodType_Id", "dbo.GoodTypes");
            DropForeignKey("dbo.SpecialPrices", "GoodType_Id", "dbo.GoodTypes");
            DropIndex("dbo.Sales", new[] { "Customer_Id" });
            DropIndex("dbo.Sales", new[] { "GoodType_Id" });
            DropIndex("dbo.SpecialPrices", new[] { "Customer_Id" });
            DropIndex("dbo.SpecialPrices", new[] { "GoodType_Id" });
            RenameColumn(table: "dbo.Sales", name: "Customer_Id", newName: "CustomerId");
            RenameColumn(table: "dbo.SpecialPrices", name: "Customer_Id", newName: "CustomerId");
            RenameColumn(table: "dbo.Sales", name: "GoodType_Id", newName: "GoodTypeId");
            RenameColumn(table: "dbo.SpecialPrices", name: "GoodType_Id", newName: "GoodTypeId");
            AlterColumn("dbo.Sales", "CustomerId", c => c.Int(nullable: false));
            AlterColumn("dbo.Sales", "GoodTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.SpecialPrices", "CustomerId", c => c.Int(nullable: false));
            AlterColumn("dbo.SpecialPrices", "GoodTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Sales", "CustomerId");
            CreateIndex("dbo.Sales", "GoodTypeId");
            CreateIndex("dbo.SpecialPrices", "CustomerId");
            CreateIndex("dbo.SpecialPrices", "GoodTypeId");
            AddForeignKey("dbo.Sales", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SpecialPrices", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Sales", "GoodTypeId", "dbo.GoodTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SpecialPrices", "GoodTypeId", "dbo.GoodTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpecialPrices", "GoodTypeId", "dbo.GoodTypes");
            DropForeignKey("dbo.Sales", "GoodTypeId", "dbo.GoodTypes");
            DropForeignKey("dbo.SpecialPrices", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Sales", "CustomerId", "dbo.Customers");
            DropIndex("dbo.SpecialPrices", new[] { "GoodTypeId" });
            DropIndex("dbo.SpecialPrices", new[] { "CustomerId" });
            DropIndex("dbo.Sales", new[] { "GoodTypeId" });
            DropIndex("dbo.Sales", new[] { "CustomerId" });
            AlterColumn("dbo.SpecialPrices", "GoodTypeId", c => c.Int());
            AlterColumn("dbo.SpecialPrices", "CustomerId", c => c.Int());
            AlterColumn("dbo.Sales", "GoodTypeId", c => c.Int());
            AlterColumn("dbo.Sales", "CustomerId", c => c.Int());
            RenameColumn(table: "dbo.SpecialPrices", name: "GoodTypeId", newName: "GoodType_Id");
            RenameColumn(table: "dbo.Sales", name: "GoodTypeId", newName: "GoodType_Id");
            RenameColumn(table: "dbo.SpecialPrices", name: "CustomerId", newName: "Customer_Id");
            RenameColumn(table: "dbo.Sales", name: "CustomerId", newName: "Customer_Id");
            CreateIndex("dbo.SpecialPrices", "GoodType_Id");
            CreateIndex("dbo.SpecialPrices", "Customer_Id");
            CreateIndex("dbo.Sales", "GoodType_Id");
            CreateIndex("dbo.Sales", "Customer_Id");
            AddForeignKey("dbo.SpecialPrices", "GoodType_Id", "dbo.GoodTypes", "Id");
            AddForeignKey("dbo.Sales", "GoodType_Id", "dbo.GoodTypes", "Id");
            AddForeignKey("dbo.SpecialPrices", "Customer_Id", "dbo.Customers", "Id");
            AddForeignKey("dbo.Sales", "Customer_Id", "dbo.Customers", "Id");
        }
    }
}
