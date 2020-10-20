using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Core.Models
{
    public interface IEmailConfiguration
    {
		string SmtpServer { get; set; }
		int SmtpPortSSL { get; set; }
		int SmtpPortTLS { get; set; }
		bool IsTLSSSLRequired { get; set; }
		string SmtpUsername { get; set; }
		string SmtpPassword { get; set; }


		string PopServer { get; set; }
		string PopRequireSSL { get; set; }
		int PortPop { get; set; }
		string PopUsername { get; set; }
		string PopPassword { get; set; }
	}
}
