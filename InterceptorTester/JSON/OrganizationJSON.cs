using System.Runtime.Serialization;

namespace ConsoleApplication1
{

	//[DataContract]

	public class OrganizationJSON
	{

<<<<<<< HEAD
		
		public int ownerID;
=======
		public bool isValid ()
		{
			if ((orgID != null) && (orgName != null))
			{
				return true;
			}

			return false;
		}

		public int orgID;
>>>>>>> parent of 9ccc7e5... Create org happy path

		public string orgName;

		// ReSharper restore InconsistentNaming

		public override string ToString()
		{
			return "";
		}
	}

}

