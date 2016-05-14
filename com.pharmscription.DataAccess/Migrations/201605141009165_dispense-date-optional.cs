namespace com.pharmscription.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dispensedateoptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Dispenses", "Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Dispenses", "Date", c => c.DateTime(nullable: false));
        }
    }
}
