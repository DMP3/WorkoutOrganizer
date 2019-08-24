namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Client", new[] { "Email" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Client", "Email", unique: true);
        }
    }
}
