namespace com.pharmscription.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public partial class drugs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IssueDate = c.DateTime(nullable: false),
                        EditDate = c.DateTime(nullable: false),
                        SignDate = c.DateTime(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        ValidUntill = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Doctor_Id = c.Guid(),
                        Patient_Id = c.Guid(),
                        Prescription_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.Doctor_Id)
                .ForeignKey("dbo.Patients", t => t.Patient_Id)
                .ForeignKey("dbo.Prescriptions", t => t.Prescription_Id)
                .Index(t => t.Doctor_Id)
                .Index(t => t.Patient_Id)
                .Index(t => t.Prescription_Id);
            
            CreateTable(
                "dbo.CounterProposals",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Message = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        Prescription_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prescriptions", t => t.Prescription_Id)
                .Index(t => t.Prescription_Id);
            
            CreateTable(
                "dbo.Dispenses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Remark = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        Prescription_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prescriptions", t => t.Prescription_Id)
                .Index(t => t.Prescription_Id);
            
            CreateTable(
                "dbo.DrugItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DosageDescription = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        Dispense_Id = c.Guid(),
                        Drug_Id = c.Guid(),
                        Prescription_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dispenses", t => t.Dispense_Id)
                .ForeignKey("dbo.Drugs", t => t.Drug_Id)
                .ForeignKey("dbo.Prescriptions", t => t.Prescription_Id)
                .Index(t => t.Dispense_Id)
                .Index(t => t.Drug_Id)
                .Index(t => t.Prescription_Id);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ZsrNumber = c.String(),
                        PhoneNumber = c.String(),
                        FaxNumber = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        Address_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .Index(t => t.Address_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prescriptions", "Prescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.Prescriptions", "Patient_Id", "dbo.Patients");
            DropForeignKey("dbo.Prescriptions", "Doctor_Id", "dbo.Doctors");
            DropForeignKey("dbo.Doctors", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Dispenses", "Prescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.DrugItems", "Prescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.DrugItems", "Drug_Id", "dbo.Drugs");
            DropForeignKey("dbo.DrugItems", "Dispense_Id", "dbo.Dispenses");
            DropForeignKey("dbo.CounterProposals", "Prescription_Id", "dbo.Prescriptions");
            DropIndex("dbo.Doctors", new[] { "Address_Id" });
            DropIndex("dbo.DrugItems", new[] { "Prescription_Id" });
            DropIndex("dbo.DrugItems", new[] { "Drug_Id" });
            DropIndex("dbo.DrugItems", new[] { "Dispense_Id" });
            DropIndex("dbo.Dispenses", new[] { "Prescription_Id" });
            DropIndex("dbo.CounterProposals", new[] { "Prescription_Id" });
            DropIndex("dbo.Prescriptions", new[] { "Prescription_Id" });
            DropIndex("dbo.Prescriptions", new[] { "Patient_Id" });
            DropIndex("dbo.Prescriptions", new[] { "Doctor_Id" });
            DropTable("dbo.Doctors");
            DropTable("dbo.DrugItems");
            DropTable("dbo.Dispenses");
            DropTable("dbo.CounterProposals");
            DropTable("dbo.Prescriptions");
        }
    }
}
