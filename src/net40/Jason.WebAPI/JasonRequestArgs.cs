using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jason.WebAPI
{
	public class JasonRequestArgs
	{
		public String CorrelationId { get; set; }
		public Boolean RequestContainsCorrelationId { get { return !String.IsNullOrWhiteSpace( this.CorrelationId ); } }
		public Boolean AppendCorrelationIdToResponse { get; set; }
		public bool IsJasonExecute { get; internal set; }
		public Boolean IsCommandInterceptor { get; internal set; }

		public System.Net.Http.HttpRequestMessage HttpRequest { get; internal set; }
	}
}
