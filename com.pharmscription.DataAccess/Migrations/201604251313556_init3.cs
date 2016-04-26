namespace com.pharmscription.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class init3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CounterProposals", "SinglePrescription_Id", c => c.Guid());
            AddColumn("dbo.CounterProposals", "StandingPrescription_Id", c => c.Guid());
            AddColumn("dbo.Dispenses", "SinglePrescription_Id", c => c.Guid());
            AddColumn("dbo.Dispenses", "StandingPrescription_Id", c => c.Guid());
            CreateIndex("dbo.CounterProposals", "SinglePrescription_Id");
            CreateIndex("dbo.CounterProposals", "StandingPrescription_Id");
            CreateIndex("dbo.Dispenses", "SinglePrescription_Id");
            CreateIndex("dbo.Dispenses", "StandingPrescription_Id");
            AddForeignKey("dbo.Dispenses", "SinglePrescription_Id", "dbo.Prescriptions", "Id");
            AddForeignKey("dbo.Dispenses", "StandingPrescription_Id", "dbo.Prescriptions", "Id");
            AddForeignKey("dbo.CounterProposals", "SinglePrescription_Id", "dbo.Prescriptions", "Id");
            AddForeignKey("dbo.CounterProposals", "StandingPrescription_Id", "dbo.Prescriptions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CounterProposals", "StandingPrescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.CounterProposals", "SinglePrescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.Dispenses", "StandingPrescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.Dispenses", "SinglePrescription_Id", "dbo.Prescriptions");
            DropIndex("dbo.Dispenses", new[] { "StandingPrescription_Id" });
            DropIndex("dbo.Dispenses", new[] { "SinglePrescription_Id" });
            DropIndex("dbo.CounterProposals", new[] { "StandingPrescription_Id" });
            DropIndex("dbo.CounterProposals", new[] { "SinglePrescription_Id" });
            DropColumn("dbo.Dispenses", "StandingPrescription_Id");
            DropColumn("dbo.Dispenses", "SinglePrescription_Id");
            DropColumn("dbo.CounterProposals", "StandingPrescription_Id");
            DropColumn("dbo.CounterProposals", "SinglePrescription_Id");
        }
    }
}
