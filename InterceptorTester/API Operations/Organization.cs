using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
	class Organization : APIOperation
	{
		public Organization(Uri server, string serialNum)
		{
			opHost = server;
			hOp = HTTPOperation.POST;
			opQuery = new HTTPQuery(QueryParameter.i, serialNum);
		}

		public override string getExpectedResult()
		{
			if (opQuery.isValid())
			{
				return "200";
			}
			else
			{
				return "400";
			}
		}

		public override string ToString()
		{
			return "Organization";
		}

		public override Uri getUri()
		{
			return new Uri(opHost, "/api/Organization/");
		}

		public override object getJson()
		{
			return null;
		}
	}
}




