using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RRISD_HAC_Access
{
    public enum SMSCarrier
    {
        ATT,
        Sprint,
        Verizon
    }
    class SMS
    {
        public bool sendSMS(String number, String content, SMSCarrier carrier, Tuple<String,String>credentials)
        {
            if (carrier == SMSCarrier.ATT)
            {
                return sendEmail(number + "@mms.att.net", content, credentials.Item1, credentials.Item2);
            }else if (carrier == SMSCarrier.Sprint)
            {
                return sendEmail(number + "@pm.sprint.com", content, credentials.Item1, credentials.Item2);
            }else if (carrier == SMSCarrier.Verizon)
            {
                return sendEmail(number + "@vzwpix.com", content, credentials.Item1, credentials.Item2);
            }
            return false;
        }
        public bool sendEmail(String address,String content, String email, String password)
        {
            try {
                MailAddress fromAddress = new MailAddress(email, "HAC SMS");
                MailAddress toAddress = new MailAddress(address, "Recepient");
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, password)
                };
                using (MailMessage message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = "Automated message from HAC SMS",
                    Body = content
                })
                {
                    smtp.Send(message);
                    return true;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
