using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Wrapper
{
    public class WorkoutWrapper : ModelWrapper<Workout>
    {
        public WorkoutWrapper(Workout model) : base(model)
        {
        }

        public int Id { get { return Model.Id; } }

        public string Title
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public DateTime Date
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Date):
                    if (Date < DateTime.Today)
                    {
                        yield return "Не може да избирате предходни дати.";
                    }
                    break;
            }
        }
    }
}
