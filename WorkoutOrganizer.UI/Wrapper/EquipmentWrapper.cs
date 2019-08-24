using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Wrapper
{
    public class EquipmentWrapper : ModelWrapper<Equipment>
    {
        public EquipmentWrapper(Equipment model) : base(model)
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
