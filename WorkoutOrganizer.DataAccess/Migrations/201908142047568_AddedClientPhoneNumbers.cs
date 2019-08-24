namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedClientPhoneNumbers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientPhoneNumber",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Client", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientPhoneNumber", "ClientId", "dbo.Client");
            DropIndex("dbo.ClientPhoneNumber", new[] { "ClientId" });
            DropTable("dbo.ClientPhoneNumber");
        }
    }
}
