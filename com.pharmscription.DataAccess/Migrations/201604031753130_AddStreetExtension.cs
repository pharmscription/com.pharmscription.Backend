namespace com.pharmscription.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStreetExtension : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "StreetExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "StreetExtension");
        }
    }
}
