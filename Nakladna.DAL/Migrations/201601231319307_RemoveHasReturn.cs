namespace Nakladna.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveHasReturn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GoodTypes", "HasReturn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GoodTypes", "HasReturn", c => c.Boolean(nullable: false));
        }
    }
}
