namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedExerciseRowVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exercise", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exercise", "RowVersion");
        }
    }
}
