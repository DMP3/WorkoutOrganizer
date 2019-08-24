using System.Threading.Tasks;

namespace WorkoutOrganizer.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int Id);
        bool HasChanges { get; }
        int Id { get; }
    }
}