using CloudnsAPI.Client.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudnsAPI.Client.Abstractions
{
	public interface IDNSRequest : IRequest<IEnumerable<DNSResponse>>
	{
	}
}
