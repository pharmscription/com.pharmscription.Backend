namespace com.pharmscription.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDrugPrice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DrugPrices",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Price = c.Double(nullable: false),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        Drug_Id = c.Guid(),
                        DrugStore_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drugs", t => t.Drug_Id)
                .ForeignKey("dbo.DrugStores", t => t.DrugStore_Id)
                .Index(t => t.Drug_Id)
                .Index(t => t.DrugStore_Id);
            
            CreateTable(
                "dbo.DrugStores",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        Address_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .Index(t => t.Address_Id);
            
            AddColumn("dbo.Dispenses", "Reported", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DrugPrices", "DrugStore_Id", "dbo.DrugStores");
            DropForeignKey("dbo.DrugStores", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.DrugPrices", "Drug_Id", "dbo.Drugs");
            DropIndex("dbo.DrugStores", new[] { "Address_Id" });
            DropIndex("dbo.DrugPrices", new[] { "DrugStore_Id" });
            DropIndex("dbo.DrugPrices", new[] { "Drug_Id" });
            DropColumn("dbo.Dispenses", "Reported");
            DropTable("dbo.DrugStores");
            DropTable("dbo.DrugPrices");
        }
    }
}
