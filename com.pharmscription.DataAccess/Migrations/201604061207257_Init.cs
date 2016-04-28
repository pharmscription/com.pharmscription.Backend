namespace com.pharmscription.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
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
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EMailAddress = c.String(),
                        AhvNumber = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        PhoneNumber = c.String(),
                        InsuranceNumber = c.String(),
                        Insurance = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        Address_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .Index(t => t.Address_Id);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Street = c.String(),
                        StreetExtension = c.String(),
                        State = c.String(),
                        Number = c.String(),
                        Location = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Patients", "Address_Id", "dbo.Addresses");
            DropIndex("dbo.Patients", new[] { "Address_Id" });
            DropTable("dbo.Addresses");
            DropTable("dbo.Patients");
            DropTable("dbo.Drugs");
        }
    }
}
