namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedExerciseDemoLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exercise", "DemoLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exercise", "DemoLink");
        }
    }
}
