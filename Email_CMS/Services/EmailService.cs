using CMS.Core.Models;
using MailKit.Net.Pop3;
using MailKit.Net.Proxy;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CMS.Core.Services
{
	public class EmailService : IEmailService
	{
		private readonly IEmailConfiguration emailConfiguration;

		public EmailService(IEmailConfiguration emailConfiguration)
		{
			this.emailConfiguration = emailConfiguration;
		}

		public bool SendMail(EmailMessage emailMessage)
		{
			var message = new MimeMessage();
			message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
			message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

			message.Subject = emailMessage.Subject;
			//We will say we are sending HTML. But there are options for plaintext etc. 
			message.Body = new TextPart(TextFormat.Html)
			{
				Text = emailMessage.Content
			};

			//Be careful that the SmtpClient class is the one from Mailkit not the framework!
			using (var emailClient = new SmtpClient())
			{
				//The last parameter here is to use SSL (Which you should!)
				emailClient.Connect(emailConfiguration.SmtpServer, emailConfiguration.SmtpPortTLS, true);

				//Remove any OAuth functionality as we won't be using it. 
				emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

				emailClient.Authenticate(emailConfiguration.SmtpUsername, emailConfiguration.SmtpPassword);

				emailClient.Send(message);

				emailClient.Disconnect(true);
			}

			return true;
		}
		
		public async Task<List<EmailMessage>> ReceiveMail(int count = 10)
		{
			try {
				using (var emailClient = new Pop3Client())
				{
					await emailClient.ConnectAsync(emailConfiguration.PopServer, emailConfiguration.PortPop);

					emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

					await emailClient.AuthenticateAsync(emailConfiguration.PopUsername, emailConfiguration.PopPassword);

					List<EmailMessage> emails = new List<EmailMessage>();
					for (int i = 0; i < emailClient.Count && i < count; i++)
					{
						var message = emailClient.GetMessage(i);
						var emailMessage = new EmailMessage
						{
							Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
							Subject = message.Subject
						};
						emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
						emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
						emails.Add(emailMessage);
					}

					return emails;
				}
			}
			catch (Exception e)
			{
				throw;
			}
		}
	}
}
