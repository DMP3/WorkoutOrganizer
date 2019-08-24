using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Wrapper
{
    public class ClientWrapper : ModelWrapper<Client>
    {
        public ClientWrapper(Client model) : base(model)
        {
        }

        public int Id { get { return Model.Id; } }

        public string FirstName
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public string LastName
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public string Email
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public string Goals
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public string Injuries
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, "Robot", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Robots are not valid clients";
                    }
                    break;
            }
        }
    }
}
