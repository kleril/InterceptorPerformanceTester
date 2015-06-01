using System.Runtime.Serialization;

namespace ConsoleApplication1
{

	//[DataContract]

	public class LocationJSON
	{
<<<<<<< HEAD

		public LocationJSON(string orgId, string unitSuite, string street, string city, string province, string country, string postalCode)
		{
=======
        public LocationJSON(string orgId, string unitSuite, string street, string city, string province, string country, string postalCode)
        {
>>>>>>> origin/master
            this.orgId = orgId;
            this.unitSuite = unitSuite;
            this.street = street;
            this.city = city;
            this.stateProvince = province;
            this.country = country;
            this.postalCode = postalCode;
        }
<<<<<<< HEAD


		public bool isValid ()
		{
			if ((orgId != null) && (unitSuite != null) && (city != null) && (stateProvince != null) && (country != null) && (postalCode != null))
			{
				return true;
			}

			return false;
		}
=======
>>>>>>> origin/master

		public string orgId;

        public string locDesc;

        public string unitSuite;

        public string street;

        public string city;

        public string stateProvince;

        public string country;

        public string postalCode;

        public decimal latitude;

        public decimal longitude;

        public string locType;

        public string locSubType;

		// ReSharper restore InconsistentNaming

		public override string ToString()
		{
			return "";
		}
	}

}


