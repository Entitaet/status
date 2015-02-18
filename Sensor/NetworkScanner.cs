using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Sensor
{
	public class NetworkScanner
	{
		public List<NetworkEntry> GetNetworkInformation(string ipRange, bool resolveNames=true, int timeout=75, bool onlyUpDevices=true)
		{
			//Setup
			List<NetworkEntry> ret=new List<NetworkEntry>();
			CountdownEvent countdown=new CountdownEvent(1);

			string[] parts=ipRange.Split(new char[] { '.', '/' }, StringSplitOptions.RemoveEmptyEntries);
			
			int netmaskBits=32-Convert.ToInt32(parts[4]); //24 is var
			if(netmaskBits>30) throw new NotSupportedException("Only netmasks lower or equal 30 are supported");

			//Convert ip Adress into uint
			uint ipBase=0;
			ipBase+=Convert.ToUInt32(parts[0])<<24;
			ipBase+=Convert.ToUInt32(parts[1])<<16;
			ipBase+=Convert.ToUInt32(parts[2])<<8;
			ipBase+=Convert.ToUInt32(parts[3]); //<<0

			uint netmask=~((1u<<netmaskBits)-1u);

			ipBase&=netmask; //ip säubern

			//Pinging
			uint countIpAdresses=((1u<<netmaskBits)-1u);

			for(uint i=1; i<countIpAdresses; i++)
			{
				Ping p=new Ping();

				//Event handler
				p.PingCompleted+=(object sender, PingCompletedEventArgs e) =>
				{
					IPAddress ipFromReply=(IPAddress)e.UserState;

					if(e.Reply!=null&&e.Reply.Status==IPStatus.Success)
					{
						if(resolveNames) //resolveNames
						{
							string name;

							try
							{
								IPHostEntry hostEntry=Dns.GetHostEntry(ipFromReply);
								name=hostEntry.HostName;
							}
							catch(SocketException)
							{
								name="";
							}

							ret.Add(new NetworkEntry(ipFromReply, e.Reply.RoundtripTime, name));
						}
						else
						{
							ret.Add(new NetworkEntry(ipFromReply, e.Reply.RoundtripTime));
						}
					}
					else if(e.Reply==null)
					{
						if(!onlyUpDevices)
						{
							ret.Add(new NetworkEntry(ipFromReply, 0, "", false));
						}
					}

					countdown.Signal();
				};

				uint ipAsUInt=ipBase+i;
				IPAddress ipAdress=new IPAddress(0xFFFFFFFF&(IPAddress.HostToNetworkOrder(ipAsUInt)>>32));
				countdown.AddCount();
				p.SendAsync(ipAdress, timeout, ipAdress);
			}

			//Wait
			countdown.Signal();
			countdown.Wait();

			return ret;
		}
	}
}
