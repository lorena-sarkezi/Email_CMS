using CMS.Core.Models;
using CMS.Data;
using CMS.Data.Common;
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

		public async Task<Email> ComposeEmail(int threadId, string content)
        {
			ConvoThread thread = await cmsDbContext.Threads
												   .FirstOrDefaultAsync(x => x.Id == threadId);
			Email latestEmail = await cmsDbContext.Emails
												  .Include(x => x.Recepients)
												  .Include(x => x.Senders)
												  .OrderByDescending(x => x.Timestamp)
												  .FirstOrDefaultAsync(x => x.ThreadId == threadId);


			Sender sender = new Sender
			{
				SenderEmail = "ntpwstvzcms@gmail.com",
				SenderName = "NTPWSTVZCMS"
			};

			Recepient recepient = new Recepient
			{
				RecepientName = thread.InitialSenderName,
				RecepientEmail = thread.InitialSenderEmail
			};


			Email newMessage = new Email
			{
				ThreadId = threadId,
				Subject = string.Join(" ", "Re:", thread.ThreadTitle),
				Timestamp = DateTime.Now,
				TextContent = messageParser.ComposeTextContent(latestEmail, content),
				HtmlContent = messageParser.ComposeHtmlContent(latestEmail, content),
				MessageContent = content,
				InResponseTo = latestEmail.ServerMessageId,
				Senders = new List<Sender> { sender },
				Recepients = new List<Recepient> { recepient }
			};


			return newMessage;
        }

		public async Task<bool> SendMail(Email email)
		{
			var message = new MimeMessage();
			message.From.AddRange(email.Senders.Select(x => new MailboxAddress(x.SenderName, x.SenderEmail)));
			message.To.AddRange(email.Recepients.Select(x => new MailboxAddress(x.RecepientName, x.RecepientEmail)));

			message.Subject = email.Subject;
			message.Body = new TextPart(TextFormat.Plain)
			{
				Text = email.TextContent
			};

			//Be careful that the SmtpClient class is the one from Mailkit not the framework!
			using (var emailClient = new SmtpClient())
			{
				//The last parameter here is to use SSL (Which you should!)
				await emailClient.ConnectAsync(emailConfiguration.SmtpServer, emailConfiguration.SmtpPortSSL, true);

				//Remove any OAuth functionality as we won't be using it. 
				emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

				await emailClient.AuthenticateAsync(emailConfiguration.SmtpUsername, emailConfiguration.SmtpPassword);

				await emailClient.SendAsync(message);

				await emailClient.DisconnectAsync(true);
			}

			cmsDbContext.Add(email);
			await cmsDbContext.SaveChangesAsync();

			return true;
		}
		
		public async Task<List<MailThreadBasic>> ReceiveAndProcessMail(int count = 100)
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

					//Fetch existing email IDs which are also among fetched messages from server
					List<string> fetchedMessageIds = mimeMessages.Select(x => x.MessageId).ToList();
					List<string> existingIds = new List<string>();
					existingIds = (await cmsDbContext.Emails.Where(x => fetchedMessageIds.Contains(x.ServerMessageId)).ToListAsync()).Select(x => x.ServerMessageId).ToList();


					List<Email> emails = new List<Email>();
					foreach (MimeMessage mimeMessage in mimeMessages)
                    {
						// Only parse messages with IDs not in DB
                        if (!existingIds.Contains(mimeMessage.MessageId))
                        {
							emails.Add(await messageParser.ConvertMimeMessageToLocalEmail(mimeMessage));
						}
						
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
	}
}
