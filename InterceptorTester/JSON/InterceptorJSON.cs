using System.Runtime.Serialization;

namespace ConsoleApplication1
{

	//[DataContract]

	public class InterceptorJSON
	{
		public InterceptorJSON(int locId, string orgId, string intSerial)
        {
            this.locId = locId;
            this.orgId = orgId;
            //this.ssid = ssid;
            //this.wpaPSK = wpaPSK;
            this.intSerial = intSerial;
        }

		public bool isValid ()
		{
			if ((locId != null) && (intSerial != null))
			{
				return true;
			}

			return false;
		}

		int locId;

        string orgId;

		//string ssid;

		//string wpaPSK;

		string intSerial;

		// ReSharper restore InconsistentNaming

		/*public override string ToString()
		{
			return locId.ToString() + " " + ssid + " " + ssid + " " + intSerial;
		}*/
	}

}



