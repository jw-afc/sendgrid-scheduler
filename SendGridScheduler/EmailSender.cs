using System;
using System.Threading.Tasks;
using System.Timers;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace SendGridScheduler
{
	public sealed class Scheduler : IDisposable
	{
		private Timer _timer;

		public static Scheduler Instance { get; } = new Scheduler();

		static Scheduler()
		{ }

		/// <summary>
		/// Default constructor
		/// </summary>
		private Scheduler()
		{ }

		public void Start()
		{
			_timer = new Timer(int.Parse(Environment.GetEnvironmentVariable("TIMER_INTERVAL_IN_SECONDS")) * 1000);
			_timer.Elapsed += _timer_Elapsed;
			_timer.Start();

			SendEmail();
		}

		private void _timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Task.Factory.StartNew(SendEmail);
		}

		/// <summary>
		/// Setup/send timed messages e.g. all daily/weekly messages inc. collated targets grouped by start-time for delayed send.  Subsequently checks whether it is time to send any of the timed messages
		/// </summary>
		private async void SendEmail()
		{
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
			Console.WriteLine(msg.Serialize());
			Console.WriteLine(response.StatusCode);
			Console.WriteLine(response.Headers);
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}
