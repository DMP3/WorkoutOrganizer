namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedWorkout : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Workout",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkoutClient",
                c => new
                    {
                        Workout_Id = c.Int(nullable: false),
                        Client_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Workout_Id, t.Client_Id })
                .ForeignKey("dbo.Workout", t => t.Workout_Id, cascadeDelete: true)
                .ForeignKey("dbo.Client", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Workout_Id)
                .Index(t => t.Client_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkoutClient", "Client_Id", "dbo.Client");
            DropForeignKey("dbo.WorkoutClient", "Workout_Id", "dbo.Workout");
            DropIndex("dbo.WorkoutClient", new[] { "Client_Id" });
            DropIndex("dbo.WorkoutClient", new[] { "Workout_Id" });
            DropTable("dbo.WorkoutClient");
            DropTable("dbo.Workout");
        }
    }
}
