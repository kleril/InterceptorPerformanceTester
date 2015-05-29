using System.Runtime.Serialization;

namespace ConsoleApplication1
{

	//[DataContract]

	public class InterceptorJSON
	{

		public bool isValid ()
		{
			if ((locId != null) && (ssid != null) && (intSerial != null))
			{
				return true;
			}

			return false;
		}

		int locId;

		string ssid;

		string wpaPSK;

		string intSerial;

		// ReSharper restore InconsistentNaming

		public override string ToString()
		{
			return "";
		}
	}

}



