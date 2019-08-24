namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUniqueParameterToClientEmail : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Client", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Client", new[] { "Email" });
        }
    }
}
