using System.Runtime.Serialization;

namespace ConsoleApplication1
{

	//[DataContract]

	public class LocationJSON
	{

		public bool isValid ()
		{
			if ((orgId != null) && (uniteSuite != null) && (city != null) && (stateProvince != null) && (country != null) && (postalCode != null))
			{
				return true;
			}

			return false;
		}

		string orgId;

		string locDesc;

		string uniteSuite;

		string street;

		string city;

		string stateProvince;

		string country;

		string postalCode;

		decimal latitude;

		decimal longitude;

		string locType;

		string locSubType;

		// ReSharper restore InconsistentNaming

		public override string ToString()
		{
			return "";
		}
	}

}


