using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Wrapper
{
    public class MusculeGroupWrapper : ModelWrapper<MusculeGroup>
    {
        public MusculeGroupWrapper(MusculeGroup model) : base(model)
        {
        }
        public int Id { get { return Model.Id; } }
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
