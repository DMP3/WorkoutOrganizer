// <auto-generated />
namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
    public sealed partial class AddedUniqueIndexToMusculeGroupAndEquipment : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddedUniqueIndexToMusculeGroupAndEquipment));
        
        string IMigrationMetadata.Id
        {
            get { return "201908211947468_AddedUniqueIndexToMusculeGroupAndEquipment"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}