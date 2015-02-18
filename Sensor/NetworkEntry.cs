using System;
using System.Net;

namespace Sensor
{
    public class NetworkEntry
    {
		public bool Up { get; private set; }
		public IPAddress IP { get; private set; }
		public long RoundtripTime { get; private set; }
		public string Hostname { get; private set; }

		public NetworkEntry(IPAddress ip, long roundtripTime, string hostname="", bool up=true)
		{
			Up=up;
			IP=ip;
			RoundtripTime=roundtripTime;
			Hostname=hostname;
		}
    }
}