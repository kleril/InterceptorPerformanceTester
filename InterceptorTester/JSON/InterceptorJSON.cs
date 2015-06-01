using System.Runtime.Serialization;

namespace ConsoleApplication1
{

	//[DataContract]

	public class InterceptorJSON
	{
		public InterceptorJSON(string locId, string ssid, string wpaPSK, string intSerial)
        {
            this.locId = locId;
            this.ssid = ssid;
            this.wpaPSK = wpaPSK;
            this.intSerial = intSerial;
        }

		public bool isValid ()
		{
			if ((locId != null) && (ssid != null) && (intSerial != null))
			{
				return true;
			}

			return false;
		}

		string locId;

		string ssid;

		string wpaPSK;

		string intSerial;

		// ReSharper restore InconsistentNaming

		public override string ToString()
		{
			return locId.ToString() + " " + ssid + " " + ssid + " " + intSerial;
		}
	}

}



