using Prism.Events;

namespace WorkoutOrganizer.UI.Event
{
    /// <summary>
    /// The int in PubSubEvent<int> is an argument
    /// </summary>
    public class OpenDetailViewEvent : PubSubEvent<OpenDetailViewEventArgs>
    {
    }
    public class OpenDetailViewEventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
