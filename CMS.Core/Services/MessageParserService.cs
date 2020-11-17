using CMS.Data;
using CMS.Data.Database;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CMS.Core.Services
{
    public class MessageParserService : IMessageParserService
    {
        private readonly CmsDbContext cmsDbContext;

        public MessageParserService(CmsDbContext cmsDbContext)
        {
            this.cmsDbContext = cmsDbContext;
        }

        public async Task<Email> ConvertMimeMessageToLocalEmail(MimeMessage mimeMessage)
		{
			List<string> splitTextBody = mimeMessage.TextBody.Split("\r\n").ToList();

			List<string> filteredTextBodyNoQuotes = splitTextBody.Where(x => x.StartsWith('>') == false).ToList();

			if (filteredTextBodyNoQuotes.Count < splitTextBody.Count)
			{
				RemoveExtraQuoteRows(ref filteredTextBodyNoQuotes);
				splitTextBody = filteredTextBodyNoQuotes;
			}

			string mailMessageTextBody = string.Join("\r\n", splitTextBody);

			ConvoThread thread = new ConvoThread();
			List<string> subjects = mimeMessage.Subject.Split(new string[] { "Re:", "Fw:", "Fwd:" }, StringSplitOptions.RemoveEmptyEntries).ToList();

			if (subjects.Count > 0)
			{

				thread = await GetConversationThread(subjects[0]);
			}

			thread.LatestMessageTimestamp = mimeMessage.Date.LocalDateTime.Date;
			

			List<Sender> senders = mimeMessage.From.Select(x => (MailboxAddress)x).Select(x => new Sender
			{
				SenderEmail = x.Address,
				SenderName = x.Name
			}).ToList();

			List<Recepient> recepients = mimeMessage.From.Select(x => (MailboxAddress)x).Select(x => new Recepient
			{
				RecepientEmail = x.Address,
				RecepientName = x.Name
			}).ToList();

			if (thread.Id == 0)
			{
				thread.InitialSenderEmail = senders.FirstOrDefault().SenderEmail;
				thread.InitialSenderName = senders.FirstOrDefault().SenderName;

			}

			

			Email email = new Email
			{
				Id = 0,
				ThreadId = thread.Id != 0 ? thread.Id : 0, 
				Subject = mimeMessage.Subject,
				Timestamp = mimeMessage.Date.LocalDateTime,
				ServerMessageId = mimeMessage.MessageId,
				InResponseTo = mimeMessage.InReplyTo,
				HtmlContent = mimeMessage.HtmlBody,
				TextContent = mimeMessage.TextBody,
				MessageContent = mailMessageTextBody,
				Recepients = recepients,
				Senders = senders,
				Thread = thread.Id == 0 ? thread : null,
				IsIncoming = senders.Count(x => x.SenderEmail == "ntpwstvzcms@gmail.com") > 0 ? false : true
			};

			return email;
		}

		// Example title: [#12] Lorem ipsum
		public async Task<ConvoThread> GetConversationThread(string subject)
		{
			ConvoThread thread = new ConvoThread();

			subject = subject.Trim();  //Remove leading and trailing whitespace
			Regex regex = new Regex("([\\[#0-9\\]])"); // ([\[#0-9\]])

			if (regex.Match(subject).Success)  //Find existing thread
			{
				List<string> subjectSplit = subject.Split(" ").ToList();
				string threadNumberStr = subjectSplit[0];

				int hashIndex = threadNumberStr.IndexOf("#");

				threadNumberStr = threadNumberStr.Substring(hashIndex + 1, threadNumberStr.Length - (hashIndex + 2));
				//threadNumberStr = threadNumberStr.Substring(0, threadNumberStr.Length - 1);

				int threadNumber = Convert.ToInt32(threadNumberStr);

				thread = await cmsDbContext.Threads.FirstOrDefaultAsync(x => x.Id == threadNumber);
			}
			else
			{
				thread.TimestampCreated = DateTime.Now;
				thread.ThreadTitle = subject;
			}

			return thread;
		}

		public void RemoveExtraQuoteRows(ref List<string> filteredTextBodyNoQuotes)
		{
			bool isFinalLineRemoved = false;
			int index = filteredTextBodyNoQuotes.Count - 1;

			while (isFinalLineRemoved == false)
			{
				if (filteredTextBodyNoQuotes[index] != "")
				{
					int localIndex = index;
					while (true)
					{
						if (filteredTextBodyNoQuotes[localIndex] != "")
						{
							localIndex--;
						}
						else
						{
							break;
						}
					}
					index = index - (index - localIndex);
					isFinalLineRemoved = true;
				}
				else
				{
					index--;
				}
			}
			int listRangeToRemove = (filteredTextBodyNoQuotes.Count - 1) - index;
			filteredTextBodyNoQuotes.RemoveRange(index, listRangeToRemove);
		}

		public string ComposeTextContent(Email email, string newMessageContent)
        {
			//Add quote to each email line
			List<string> emailLines = email.TextContent.Split("\r\n").ToList();
			for (int i = 0; i < emailLines.Count; i++)
			{
				emailLines[i] = $">{emailLines[i]}";
			}

			string tStampDay = email.Timestamp.ToString("ddd, MMMM dd, yyyy");
			string tStampTime = email.Timestamp.ToString("HH:mm");
			string emailLinesJoined = String.Join("\r\n", emailLines);
			string senderEmail = email.Senders.FirstOrDefault().SenderEmail;
			string senderName = email.Senders.FirstOrDefault().SenderName;

			string messageNew = $"{newMessageContent}\r\n\r\nOn {tStampDay} at {tStampTime} {senderName} <{senderEmail}>\r\nwrote:\r\n\r\n{emailLinesJoined}";

			return messageNew;
		}

		public string ComposeHtmlContent(Email email, string newMessageContent)
		{
			string tStampDay = email.Timestamp.ToString("ddd, MMMM dd, yyyy");
			string tStampTime = email.Timestamp.ToString("HH:mm");
			string senderEmail = email.Senders.FirstOrDefault().SenderEmail;
			string senderName = email.Senders.FirstOrDefault().SenderName;

			var doc = new HtmlDocument();

			HtmlNode msgContent = doc.CreateElement("div");
			msgContent.InnerHtml = newMessageContent;
			msgContent.Attributes.Add("dir", "ltr");

			HtmlNode br = doc.CreateElement("br");

			HtmlNode quote = doc.CreateElement("div");
			quote.AddClass("gmail_quote");

			string mailHref = $"<a href=\"mailto:{senderEmail}\">{senderName}</a>";

			HtmlNode onDateWrote = doc.CreateElement("div");
			onDateWrote.Attributes.Add("dir", "ltr");
			onDateWrote.AddClass("gmail_attr");
			onDateWrote.InnerHtml = $"On  {tStampDay} on {tStampTime} {senderName} &lt;{mailHref}&gt; wrote:<br>";

			HtmlNode blockquote = doc.CreateElement("blockquote");
			blockquote.AddClass("gmail_quote");
			blockquote.Attributes.Add("style", "margin:0px 0px 0px 0.8ex;border-left:1px solid rgb(204,204,204);padding-left:1ex");
			blockquote.InnerHtml = email.HtmlContent;

			quote.AppendChild(onDateWrote);
			quote.AppendChild(blockquote);

			doc.DocumentNode.AppendChild(msgContent);
			doc.DocumentNode.AppendChild(br);
			doc.DocumentNode.AppendChild(quote);

			return doc.DocumentNode.OuterHtml;
        }
	}
}
