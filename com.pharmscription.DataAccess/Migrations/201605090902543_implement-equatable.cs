namespace com.pharmscription.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class implementequatable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prescriptions", "SignDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prescriptions", "SignDate");
        }
    }
}
