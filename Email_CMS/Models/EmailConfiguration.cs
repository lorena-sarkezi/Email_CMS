using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Core.Models
{
    public class EmailConfiguration : IEmailConfiguration
    {
		public string SmtpServer { get; set; }
		public int SmtpPortSSL { get; set; }
		public int SmtpPortTLS { get; set; }
		public bool IsTLSSSLRequired { get; set; }
		public string SmtpUsername { get; set; }
		public string SmtpPassword { get; set; }


		public string PopServer { get; set; }
		public string PopRequireSSL { get; set; }
		public int PortPop { get; set; }
		public string PopUsername { get; set; }
		public string PopPassword { get; set; }
	}
}
