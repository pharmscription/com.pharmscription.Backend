namespace com.pharmscription.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddPatientInsurance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
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
        }
    }
}
