using CMS.Core.Models;
using CMS.Data;
using CMS.Data.Common;
using CMS.Models.Database;
using MailKit.Net.Pop3;
using MailKit.Net.Proxy;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
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
        private readonly CmsDbContext cmsDbContext;

        public EmailService(IEmailConfiguration emailConfiguration, CmsDbContext cmsDbContext)
		{
			this.emailConfiguration = emailConfiguration;
            this.cmsDbContext = cmsDbContext;
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
							Subject = message.Subject,
							Timestamp = message.Date.ToLocalTime().DateTime,
							Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody

						};
						emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
						emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
						emails.Add(emailMessage);
					}


					List<Email> emailsDb = new List<Email>();
					foreach(EmailMessage message in emails)
                    {
						Email emailTemp = new Email
						{
							Subject = message.Subject,
							Timestamp = message.Timestamp,
							SenderEmail = message.FromAddresses.FirstOrDefault()?.Address,
							SenderName = message.FromAddresses.FirstOrDefault()?.Name,
							RecepientEmail = message.ToAddresses.FirstOrDefault()?.Address,
							RecepientName = message.ToAddresses.FirstOrDefault()?.Name,
							Content = message.Content
						};

						emailsDb.Add(emailTemp);
                    }

					cmsDbContext.AddRange(emailsDb);
					await cmsDbContext.SaveChangesAsync();

					return emails;
				}
			}
			catch (Exception e)
			{
				throw;
			}
		}

		public async Task<List<MailThreadBasic>> GetBasicMailThreads()
        {
			return await cmsDbContext.Emails.Select(x => x.ToMailThreadBasic()).ToListAsync();
        }
	}
}
