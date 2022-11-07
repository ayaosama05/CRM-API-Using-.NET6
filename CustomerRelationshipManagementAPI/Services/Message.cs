using Microsoft.IdentityModel.Tokens;

namespace CustomerRelationshipManagement.API.Services
{
    public class Message : MessageContract
    {
        public string PhoneNumber { get; protected set; }
        public string TextMessage { get; protected set; }


        public Message(string phoneNumber, string textMessage)
        {
            this.PhoneNumber = phoneNumber;
            this.TextMessage = textMessage;

        }

        public override bool IsValid()
        {

            if (this.PhoneNumber.IsNullOrEmpty())
            {
                base.Errors.Add("Phone number is empty");
                return false;
            }

            if (this.TextMessage.IsNullOrEmpty())
            {
                base.Errors.Add("The message is empty");
                return false;
            }

            if (this.TextMessage.Length > 160)
            {
                base.Errors.Add("The message exceeded 160 characters");
                return false;
            }

            return true;
        }


    }
}
