namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedExerciseSetups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExerciseSetup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Position = c.Int(nullable: false),
                        Sets = c.String(maxLength: 10),
                        Reps = c.String(maxLength: 10),
                        Weight = c.String(maxLength: 10),
                        Rest = c.String(maxLength: 10),
                        ExerciseId = c.Int(nullable: false),
                        WorkoutId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exercise", t => t.ExerciseId, cascadeDelete: true)
                .ForeignKey("dbo.Workout", t => t.WorkoutId, cascadeDelete: true)
                .Index(t => t.ExerciseId)
                .Index(t => t.WorkoutId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExerciseSetup", "WorkoutId", "dbo.Workout");
            DropForeignKey("dbo.ExerciseSetup", "ExerciseId", "dbo.Exercise");
            DropIndex("dbo.ExerciseSetup", new[] { "WorkoutId" });
            DropIndex("dbo.ExerciseSetup", new[] { "ExerciseId" });
            DropTable("dbo.ExerciseSetup");
        }
    }
}
