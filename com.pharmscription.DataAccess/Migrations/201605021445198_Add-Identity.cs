namespace com.pharmscription.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IdentityUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        IdentityUser_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IdentityUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            AddColumn("dbo.Doctors", "User_Id", c => c.Guid());
            AddColumn("dbo.Patients", "User_Id", c => c.Guid());
            CreateIndex("dbo.Doctors", "User_Id");
            CreateIndex("dbo.Patients", "User_Id");
            AddForeignKey("dbo.Doctors", "User_Id", "dbo.IdentityUsers", "Id");
            AddForeignKey("dbo.Patients", "User_Id", "dbo.IdentityUsers", "Id");
            DropColumn("dbo.DrugItems", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DrugItems", "Quantity", c => c.Int(nullable: false));
            DropForeignKey("dbo.Patients", "User_Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.Doctors", "User_Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.IdentityRoles", "IdentityUser_Id", "dbo.IdentityUsers");
            DropIndex("dbo.Patients", new[] { "User_Id" });
            DropIndex("dbo.IdentityRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.Doctors", new[] { "User_Id" });
            DropColumn("dbo.Patients", "User_Id");
            DropColumn("dbo.Doctors", "User_Id");
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.IdentityUsers");
        }
    }
}
