namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEquipment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Equipment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Exercise", "EquipmentId", c => c.Int());
            CreateIndex("dbo.Exercise", "EquipmentId");
            AddForeignKey("dbo.Exercise", "EquipmentId", "dbo.Equipment", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exercise", "EquipmentId", "dbo.Equipment");
            DropIndex("dbo.Exercise", new[] { "EquipmentId" });
            DropColumn("dbo.Exercise", "EquipmentId");
            DropTable("dbo.Equipment");
        }
    }
}
