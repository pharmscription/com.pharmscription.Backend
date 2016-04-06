namespace com.pharmscription.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedDrugs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drugs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DrugDescription = c.String(),
                        PackageSize = c.String(),
                        Unit = c.String(),
                        Composition = c.String(),
                        NarcoticCategory = c.String(),
                        IsValid = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Drugs");
        }
    }
}
