using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SendGridMailer
{
	public static class Mailer
	{
		[FunctionName("Mailer")]
		public static async void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
		{
			log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

			var apiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
			var client = new SendGridClient(apiKey);

			// Send a Single Email using the Mail Helper with convenience methods and initialized SendGridMessage object
			var msg = new SendGridMessage()
			{
				From = new EmailAddress("no-reply@sendgrid.com"),
				Subject = "POP3 Update Frequency Fix",
				HtmlContent = "<strong>POP3 Update Frequency Fix</strong>"
			};
			msg.AddTo(new EmailAddress(Environment.GetEnvironmentVariable("RECIPIENT")));

			var response = await client.SendEmailAsync(msg);

			log.LogInformation(msg.Serialize());
			log.LogInformation($"StatusCode: {response.StatusCode}");
			log.LogInformation($"Headers: {response.Headers}");
		}
	}
}
