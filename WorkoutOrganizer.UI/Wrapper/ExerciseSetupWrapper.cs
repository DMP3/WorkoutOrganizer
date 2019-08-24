using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Wrapper
{
    public class ExerciseSetupWrapper : ModelWrapper<ExerciseSetup>
    {
        public ExerciseSetupWrapper(ExerciseSetup model) : base(model)
        {
        }
        public int Id { get { return Model.Id; } }
        public int Position
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string Sets
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Reps
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public string Weight
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public string Rest
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public int? WorkoutId
        {
            get { return GetValue<int?>(); }
            set { SetValue(value); }
        }
        public int? ExerciseId
        {
            get { return GetValue<int?>(); }
            set { SetValue(value); }
        }
    }
}
