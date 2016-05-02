namespace com.pharmscription.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedQuantitytoDrug : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DrugItems", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DrugItems", "Quantity");
        }
    }
}
