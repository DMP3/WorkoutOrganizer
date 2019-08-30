namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedColumnLengthRestrictions : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClientPhoneNumber", "Number", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Client", "Goals", c => c.String(maxLength: 200));
            AlterColumn("dbo.Client", "Injuries", c => c.String(maxLength: 200));
            AlterColumn("dbo.Exercise", "DemoLink", c => c.String(maxLength: 2083));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Exercise", "DemoLink", c => c.String());
            AlterColumn("dbo.Client", "Injuries", c => c.String(maxLength: 50));
            AlterColumn("dbo.Client", "Goals", c => c.String(maxLength: 50));
            AlterColumn("dbo.ClientPhoneNumber", "Number", c => c.String(nullable: false));
        }
    }
}
