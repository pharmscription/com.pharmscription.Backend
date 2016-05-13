namespace com.pharmscription.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public partial class init6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DrugItems", "SinglePrescription_Id", c => c.Guid());
            AddColumn("dbo.DrugItems", "StandingPrescription_Id", c => c.Guid());
            CreateIndex("dbo.DrugItems", "SinglePrescription_Id");
            CreateIndex("dbo.DrugItems", "StandingPrescription_Id");
            AddForeignKey("dbo.DrugItems", "SinglePrescription_Id", "dbo.Prescriptions", "Id");
            AddForeignKey("dbo.DrugItems", "StandingPrescription_Id", "dbo.Prescriptions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DrugItems", "StandingPrescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.DrugItems", "SinglePrescription_Id", "dbo.Prescriptions");
            DropIndex("dbo.DrugItems", new[] { "StandingPrescription_Id" });
            DropIndex("dbo.DrugItems", new[] { "SinglePrescription_Id" });
            DropColumn("dbo.DrugItems", "StandingPrescription_Id");
            DropColumn("dbo.DrugItems", "SinglePrescription_Id");
        }
    }
}
