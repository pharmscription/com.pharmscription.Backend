namespace com.pharmscription.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStateEMail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "EMailAddress", c => c.String());
            AddColumn("dbo.Addresses", "State", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "State");
            DropColumn("dbo.Patients", "EMailAddress");
        }
    }
}
