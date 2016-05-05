namespace com.pharmscription.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removesigndate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Prescriptions", "SignDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Prescriptions", "SignDate", c => c.DateTime());
        }
    }
}
