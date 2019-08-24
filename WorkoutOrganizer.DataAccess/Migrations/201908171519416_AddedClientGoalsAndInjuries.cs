namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedClientGoalsAndInjuries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Client", "Goals", c => c.String(maxLength: 50));
            AddColumn("dbo.Client", "Injuries", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Client", "Injuries");
            DropColumn("dbo.Client", "Goals");
        }
    }
}
