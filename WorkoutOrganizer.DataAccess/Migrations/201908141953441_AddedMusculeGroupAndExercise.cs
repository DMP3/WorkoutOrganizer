namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMusculeGroupAndExercise : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Exercise",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 70),
                        Description = c.String(maxLength: 120),
                        MusculeGroupId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MusculeGroup", t => t.MusculeGroupId)
                .Index(t => t.MusculeGroupId);
            
            CreateTable(
                "dbo.MusculeGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exercise", "MusculeGroupId", "dbo.MusculeGroup");
            DropIndex("dbo.Exercise", new[] { "MusculeGroupId" });
            DropTable("dbo.MusculeGroup");
            DropTable("dbo.Exercise");
        }
    }
}
