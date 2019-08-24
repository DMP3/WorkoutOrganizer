namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRowVersionForClient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Client", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Client", "RowVersion");
        }
    }
}
