using System.Runtime.Serialization;

namespace ConsoleApplication1
{

	//[DataContract]

	public class OrganizationJSON
	{

		
		public int ownerID;
		public bool isValid ()
		{
			if ((orgID != null) && (orgName != null))
			{
				return true;
			}

			return false;
		}

		public string orgID;

		public string orgName;

		// ReSharper restore InconsistentNaming

		public override string ToString()
		{
			return "";
		}
	}

}

