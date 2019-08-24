namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedWorkoutRowVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Workout", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Workout", "RowVersion");
        }
    }
}
