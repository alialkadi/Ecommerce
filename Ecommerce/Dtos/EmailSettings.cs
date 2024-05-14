using System.Net.Mail;
using System.Net;

namespace Ecommerce.APIs.Dtos
{
	public class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;

			//																cmha cehi jscg vhmn

			client.Credentials = new NetworkCredential("alikadie990@gmail.com", "cmha cehi jscg vhmn");
			client.Send("alikadie990@gmail.com", email.Recioient, email.Subject, email.Body);

		}
	}
}
