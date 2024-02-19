using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

try
{
  Console.WriteLine("SMTP Tester");
  Console.WriteLine("...........");
  Console.WriteLine();

  var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

  var recipientEmail = config["to"] ?? string.Empty;
  var fromEmail = config["from"] ?? string.Empty;
  var smtpServer = config["server"] ?? string.Empty;
  var smtpPort = Convert.ToInt32(config["port"]);

  Console.Write($"SMTP server hostname ({smtpServer}): ");

  var smtp = Console.ReadLine();
  if (!string.IsNullOrEmpty(smtp))
  {
    smtpServer = smtp;
  }

  Console.Write($"SMTP server port ({smtpPort}): ");

  var port = Console.ReadLine();
  if (!string.IsNullOrEmpty(port))
  {
    smtpPort = Convert.ToInt32(port);
  }

  Console.Write($"From ({fromEmail}): ");

  var from = Console.ReadLine();
  if (!string.IsNullOrEmpty(from))
  {
    fromEmail = from;
  }

  Console.Write($"To ({recipientEmail}): ");

  var email = Console.ReadLine();
  if (!string.IsNullOrEmpty(email))
  {
    recipientEmail = email;
  }

  using var mailer = new SmtpClient();
  mailer.DeliveryMethod = SmtpDeliveryMethod.Network;
  mailer.Host = smtpServer;
  mailer.Port = smtpPort;

  using var msg = new MailMessage();
  msg.From = new MailAddress(fromEmail);

  msg.To.Add(new MailAddress(recipientEmail));

  msg.Subject = $"Test Email {DateTime.Now}";
  msg.Body = $"This is a test of SMTP server {smtpServer} from {Dns.GetHostName()}";

  mailer.Send(msg);

  Console.WriteLine("Test email was sent");
}
catch (Exception ex)
{
  Console.WriteLine($"Test email failed... {ex.Message}");
}

Console.WriteLine("Press ENTER to close");
Console.ReadLine();
