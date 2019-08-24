using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Wrapper
{
    public class ExerciseWrapper : ModelWrapper<Exercise>
    {
        public ExerciseWrapper(Exercise model) : base(model)
        {
        }
        public int Id { get { return Model.Id; } }
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public string Description
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public string DemoLink
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public int? MusculeGroupId
        {
            get { return GetValue<int?>(); }
            set { SetValue(value); }
        }
        public int? EquipmentId
        {
            get { return GetValue<int?>(); }
            set { SetValue(value); }
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(DemoLink):
                    Uri uriResult;
                    bool result = Uri.TryCreate(DemoLink, UriKind.Absolute, out uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                    if (!result)
                    {
                        yield return "Url е невалиден.";
                    }
                    break;
            }
        }
    }
}
