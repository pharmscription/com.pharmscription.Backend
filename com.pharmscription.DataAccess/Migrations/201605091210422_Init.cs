namespace com.pharmscription.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
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
                        SinglePrescription_Id = c.Guid(),
                        StandingPrescription_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prescriptions", t => t.Prescription_Id)
                .ForeignKey("dbo.Prescriptions", t => t.SinglePrescription_Id)
                .ForeignKey("dbo.Prescriptions", t => t.StandingPrescription_Id)
                .Index(t => t.Prescription_Id)
                .Index(t => t.SinglePrescription_Id)
                .Index(t => t.StandingPrescription_Id);
            
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IssueDate = c.DateTime(nullable: false),
                        EditDate = c.DateTime(nullable: false),
                        SignDate = c.DateTime(),
                        IsValid = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        ValidUntill = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Patient_Id = c.Guid(),
                        Doctor_Id = c.Guid(),
                        Prescription_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patients", t => t.Patient_Id)
                .ForeignKey("dbo.Doctors", t => t.Doctor_Id)
                .ForeignKey("dbo.Prescriptions", t => t.Prescription_Id)
                .Index(t => t.Patient_Id)
                .Index(t => t.Doctor_Id)
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
                        SinglePrescription_Id = c.Guid(),
                        StandingPrescription_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prescriptions", t => t.Prescription_Id)
                .ForeignKey("dbo.Prescriptions", t => t.SinglePrescription_Id)
                .ForeignKey("dbo.Prescriptions", t => t.StandingPrescription_Id)
                .Index(t => t.Prescription_Id)
                .Index(t => t.SinglePrescription_Id)
                .Index(t => t.StandingPrescription_Id);
            
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
                        SinglePrescription_Id = c.Guid(),
                        StandingPrescription_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dispenses", t => t.Dispense_Id)
                .ForeignKey("dbo.Drugs", t => t.Drug_Id)
                .ForeignKey("dbo.Prescriptions", t => t.Prescription_Id)
                .ForeignKey("dbo.Prescriptions", t => t.SinglePrescription_Id)
                .ForeignKey("dbo.Prescriptions", t => t.StandingPrescription_Id)
                .Index(t => t.Dispense_Id)
                .Index(t => t.Drug_Id)
                .Index(t => t.Prescription_Id)
                .Index(t => t.SinglePrescription_Id)
                .Index(t => t.StandingPrescription_Id);
            
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
                "dbo.Doctors",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ZsrNumber = c.String(),
                        PhoneNumber = c.String(),
                        FaxNumber = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Role = c.String(),
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
                        CityCode_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbstractCityCodes", t => t.CityCode_Id)
                .Index(t => t.CityCode_Id);
            
            CreateTable(
                "dbo.AbstractCityCodes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CityCode = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EMailAddress = c.String(),
                        AhvNumber = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        PhoneNumber = c.String(),
                        InsuranceNumber = c.String(),
                        Insurance = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Role = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        Address_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .Index(t => t.Address_Id);
            
            CreateTable(
                "dbo.Drugists",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Role = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DrugstoreEmployees",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Role = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CounterProposals", "StandingPrescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.CounterProposals", "SinglePrescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.Prescriptions", "Prescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.Prescriptions", "Doctor_Id", "dbo.Doctors");
            DropForeignKey("dbo.Dispenses", "StandingPrescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.Dispenses", "SinglePrescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.Dispenses", "Prescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.DrugItems", "StandingPrescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.DrugItems", "SinglePrescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.Prescriptions", "Patient_Id", "dbo.Patients");
            DropForeignKey("dbo.Patients", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Doctors", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Addresses", "CityCode_Id", "dbo.AbstractCityCodes");
            DropForeignKey("dbo.DrugItems", "Prescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.DrugItems", "Drug_Id", "dbo.Drugs");
            DropForeignKey("dbo.DrugItems", "Dispense_Id", "dbo.Dispenses");
            DropForeignKey("dbo.CounterProposals", "Prescription_Id", "dbo.Prescriptions");
            DropIndex("dbo.Patients", new[] { "Address_Id" });
            DropIndex("dbo.Addresses", new[] { "CityCode_Id" });
            DropIndex("dbo.Doctors", new[] { "Address_Id" });
            DropIndex("dbo.DrugItems", new[] { "StandingPrescription_Id" });
            DropIndex("dbo.DrugItems", new[] { "SinglePrescription_Id" });
            DropIndex("dbo.DrugItems", new[] { "Prescription_Id" });
            DropIndex("dbo.DrugItems", new[] { "Drug_Id" });
            DropIndex("dbo.DrugItems", new[] { "Dispense_Id" });
            DropIndex("dbo.Dispenses", new[] { "StandingPrescription_Id" });
            DropIndex("dbo.Dispenses", new[] { "SinglePrescription_Id" });
            DropIndex("dbo.Dispenses", new[] { "Prescription_Id" });
            DropIndex("dbo.Prescriptions", new[] { "Prescription_Id" });
            DropIndex("dbo.Prescriptions", new[] { "Doctor_Id" });
            DropIndex("dbo.Prescriptions", new[] { "Patient_Id" });
            DropIndex("dbo.CounterProposals", new[] { "StandingPrescription_Id" });
            DropIndex("dbo.CounterProposals", new[] { "SinglePrescription_Id" });
            DropIndex("dbo.CounterProposals", new[] { "Prescription_Id" });
            DropTable("dbo.DrugstoreEmployees");
            DropTable("dbo.Drugists");
            DropTable("dbo.Patients");
            DropTable("dbo.AbstractCityCodes");
            DropTable("dbo.Addresses");
            DropTable("dbo.Doctors");
            DropTable("dbo.Drugs");
            DropTable("dbo.DrugItems");
            DropTable("dbo.Dispenses");
            DropTable("dbo.Prescriptions");
            DropTable("dbo.CounterProposals");
        }
    }
}
