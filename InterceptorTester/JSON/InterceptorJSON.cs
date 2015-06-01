using System.Runtime.Serialization;

namespace ConsoleApplication1
{

	//[DataContract]

	public class InterceptorJSON
	{
        /*
        public InterceptorJSON(int locId, string ssid, string wpaPSK, string intSerial)
        {
            this.locId = locId;
            this.ssid = ssid;
            this.wpaPSK = wpaPSK;
            this.intSerial = intSerial;
        }
        */
        public InterceptorJSON(int locId, string orgId, string intSerial)
        {
            this.locId = locId;
            this.orgId = orgId;
            this.intSerial = intSerial;
        }

		public int locId;

        public string orgId;

		//string ssid;

		//string wpaPSK;

		public string intSerial;

		// ReSharper restore InconsistentNaming

		public override string ToString()
		{
			return locId.ToString() + " " + intSerial;
		}
	}

}



