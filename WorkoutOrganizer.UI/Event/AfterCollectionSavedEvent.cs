using Prism.Events;

namespace WorkoutOrganizer.UI.Event
{
    public class AfterCollectionSavedEvent : PubSubEvent<AfterCollectionSavedEventArgs>
    {
    }
    public class AfterCollectionSavedEventArgs
    {
        public string ViewModelName { get; set; }
    }
}
