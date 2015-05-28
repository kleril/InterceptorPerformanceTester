using System.Runtime.Serialization;

namespace ConsoleApplication1
{

	//[DataContract]

	public class OrganizationJSON
	{

		public bool isValid ()
		{
			if ((orgID != null) && (orgName != null))
			{
				return true;
			}

			return false;
		}

		int orgID;

		string orgName;

		// ReSharper restore InconsistentNaming

		public override string ToString()
		{
			return "";
		}
	}

}

