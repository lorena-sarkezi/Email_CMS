using CMS.Data;
using CMS.Data.Database;
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

			Email email = new Email
			{
				Subject = mimeMessage.Subject,
				HtmlContent = mimeMessage.HtmlBody,
				TextContent = mimeMessage.TextBody,
				MessageContent = mailMessageTextBody,
				Recepients = recepients,
				Senders = senders,
				Thread = thread
				
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

				threadNumberStr = threadNumberStr.Substring(hashIndex + 1, threadNumberStr.Length - 1);
				threadNumberStr = threadNumberStr.Substring(0, threadNumberStr.Length - 1);

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
	}
}
