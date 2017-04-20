using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SendGridScheduler
{
	class Program
	{
		static void Main(string[] args)
		{
			Scheduler.Instance.Start();

			Console.WriteLine("Running. Press any key to stop...");
			Console.ReadLine();
		}
	}
}
