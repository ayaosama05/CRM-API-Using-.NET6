using Twilio.TwiML.Messaging;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using static Twilio.Rest.Api.V2010.Account.Call.FeedbackSummaryResource;

namespace CustomerRelationshipManagement.API.Services
{
    public class TwilioService:ITwilioService
    {
        private readonly IConfiguration _configuration;

        public TwilioService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public ServiceResponse Send(Message message)
        {
            ServiceResponse response = new ServiceResponse();

            if (message.IsValid())
            {
                TwilioCredentials twilioCredentials = new TwilioCredentials
                {
                     AccountSid = _configuration["Twillio:AccountSID"],
                     AuthToken = _configuration["Twillio:AuthToken"],
                     SenderPhoneNumber = _configuration["Twillio:SenderPhoneNumber"]
                };

                TwilioClient.Init(twilioCredentials.AccountSid, twilioCredentials.AuthToken);

                var result = MessageResource.Create(
                                            to: new PhoneNumber($"whatsapp:{message.PhoneNumber}"),
                                            from: new PhoneNumber($"whatsapp:{twilioCredentials.SenderPhoneNumber}"),
                                            body: message.TextMessage);

                if (result.Status != StatusEnum.Failed)
                {
                    response.IsValid = true;
                }
                else
                {
                    response.Errors.Add("Whats message failed!");
                    response.IsValid = false;
                }
            }
            else
            {
                response.Errors = message.Errors;
                response.IsValid = false;
            }
            return response;
        }
    }
}
