namespace com.pharmscription.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MAdeSigndateoptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Prescriptions", "SignDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Prescriptions", "SignDate", c => c.DateTime(nullable: false));
        }
    }
}
