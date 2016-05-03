namespace com.pharmscription.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public partial class ConnectedCityCodetoPatients : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.Addresses", "CityCode_Id", c => c.Guid());
            CreateIndex("dbo.Addresses", "CityCode_Id");
            AddForeignKey("dbo.Addresses", "CityCode_Id", "dbo.AbstractCityCodes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Addresses", "CityCode_Id", "dbo.AbstractCityCodes");
            DropIndex("dbo.Addresses", new[] { "CityCode_Id" });
            DropColumn("dbo.Addresses", "CityCode_Id");
            DropTable("dbo.AbstractCityCodes");
        }
    }
}
