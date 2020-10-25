using CMS.Core.Models;
using CMS.Data;
using CMS.Data.Common;
using CMS.Data.Database;
using CMS.Data.Database;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CMS.Core.Services
{
	public class EmailService : IEmailService
	{
		private readonly IEmailConfiguration emailConfiguration;
        private readonly CmsDbContext cmsDbContext;
        private readonly IMessageParserService messageParser;

        public EmailService(IEmailConfiguration emailConfiguration, CmsDbContext cmsDbContext, IMessageParserService messageParser)
		{
			this.emailConfiguration = emailConfiguration;
            this.cmsDbContext = cmsDbContext;
            this.messageParser = messageParser;
        }

		public bool SendMail(Email email)
		{
			var message = new MimeMessage();
			message.To.AddRange(email.Senders.Select(x => new MailboxAddress(x.SenderName, x.SenderEmail)));
			message.From.AddRange(email.Recepients.Select(x => new MailboxAddress(x.RecepientName, x.RecepientEmail)));

			message.Subject = String.Join(" ", email.Thread.ThreadTitle,"Re:");
			message.Body = new TextPart(TextFormat.Html)
			{
				Text = email.MessageContent
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
		
		public async Task<List<MailThreadBasic>> ReceiveMail(int count = 10)
		{
			try {
				using (var emailClient = new Pop3Client())
				{
					//Email latestMsg = await cmsDbContext.Emails.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();

					await emailClient.ConnectAsync(emailConfiguration.PopServer, emailConfiguration.PortPop);

					emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

					await emailClient.AuthenticateAsync(emailConfiguration.PopUsername, emailConfiguration.PopPassword);

					List<MimeMessage> mimeMessages = new List<MimeMessage>();

					for (int i = 0; i < emailClient.Count && i < count; i++)
					{
						var message = await emailClient.GetMessageAsync(i);
						mimeMessages.Add(message);
					}


					List<Email> emails = new List<Email>();
					foreach (MimeMessage mimeMessage in mimeMessages)
                    {
						emails.Add(await messageParser.ConvertMimeMessageToLocalEmail(mimeMessage));
                    }

					cmsDbContext.AddRange(emails);
					await cmsDbContext.SaveChangesAsync();

					return emails.Select(x => x.ToMailThreadBasic()).ToList();
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
