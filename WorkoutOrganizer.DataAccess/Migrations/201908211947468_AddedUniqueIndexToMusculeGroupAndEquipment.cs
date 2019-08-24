namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUniqueIndexToMusculeGroupAndEquipment : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Equipment", "Name", unique: true);
            CreateIndex("dbo.MusculeGroup", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.MusculeGroup", new[] { "Name" });
            DropIndex("dbo.Equipment", new[] { "Name" });
        }
    }
}
