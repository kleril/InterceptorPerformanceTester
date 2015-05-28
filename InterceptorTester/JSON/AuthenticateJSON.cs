using System.Runtime.Serialization;

namespace ConsoleApplication1
{

	//[DataContract]

	public class AuthenticateJSON
	{

		public bool isValid ()
		{
			if ((userID != null) && (password != null))
			{
				return true;
			}

			return false;
		}

		string userID;

		string password;

		// ReSharper restore InconsistentNaming

		public override string ToString()
		{
		}
	}



}
