using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Wrapper
{
    public class ClientPhoneNumberWrapper : ModelWrapper<ClientPhoneNumber>
    {
        public ClientPhoneNumberWrapper(ClientPhoneNumber model) : base(model)
        {
        }

        public string Number
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
