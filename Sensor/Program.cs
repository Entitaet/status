using System;
using System.Collections.Generic;
using CSCL;
using System.IO;
using System.Net;
using System.Data;
using CSCL.Network.REST;
using System.Xml;

namespace Sensor
{
    class Program
    {
        public static void Main(string[] args)
        {
			//Console
			Console.WriteLine("Initializing sensor...");

            //Load config
            XmlData config;

            try
            {
                config=new XmlData("sensor.xml");
            }
            catch(Exception e)
            {
                Console.WriteLine("Konfiguration konnte nicht gelesen werden.");
                Console.WriteLine(e.ToString());
                return;
            }

            bool miscCheckCertificates=Convert.ToBoolean(config.GetElementAsString("xml.misc.checkcertificates"));

			if(miscCheckCertificates==false)
            {
                //Disable certificate check
                ServicePointManager.ServerCertificateValidationCallback=delegate
                {
                    return true;
                };
            }

			bool verbose=Convert.ToBoolean(config.GetElementAsString("xml.misc.verbose"));

            string apiToken=config.GetElementAsString("xml.api.token");
            string apiUrl=config.GetElementAsString("xml.api.url");

			//EAPI Setup
			EAPI eAPI=new EAPI(apiUrl, apiToken);

			#region Network scan
			Console.WriteLine("Scan network...");

			//Scan network
			Dictionary<string, List<NetworkEntry>> networkScans=new Dictionary<string, List<NetworkEntry>>();
			List<XmlNode> networkSegments=config.GetElements("xml.network.segment");

			foreach(XmlNode node in networkSegments)
			{
				bool active=Convert.ToBoolean(node["active"].InnerText);
				if(!active) continue;

				string key=node.Attributes["key"].Value;
				Console.WriteLine("Scan network segment {0}...", key);

				//IP Ranges
				List<string> segmentIPRanges=new List<string>();
				foreach(XmlNode childNode in node.ChildNodes)
				{
					if(childNode.LocalName=="iprange") segmentIPRanges.Add(childNode.InnerText);
				}

				//Scan ranges
				List<NetworkEntry> entries=new List<NetworkEntry>();

				foreach(string segmentIPRange in segmentIPRanges)
				{
					NetworkScanner scanner=new NetworkScanner();

					List<NetworkEntry> segmentEntries=scanner.GetNetworkInformation(segmentIPRange);
					Console.WriteLine("Detected network devices in segment {0}/{1}: {2}", key, segmentIPRange, segmentEntries.Count);

					if(verbose)
					{
						foreach(NetworkEntry networkEntry in segmentEntries)
						{
							Console.WriteLine("Detected network device: {0} / {1} ({2}, {3} ms)", networkEntry.IP, networkEntry.Hostname, networkEntry.Up?"online":"offline", networkEntry.RoundtripTime);
						}
					}

					entries.AddRange(segmentEntries);
				}

				networkScans.Add(key, entries);
			}
			#endregion

			#region Detection
			Console.WriteLine("Start detection...");

			//Entity Detection
			int countEntities=0;

			bool entityDetection=Convert.ToBoolean(config.GetElementAsString("xml.detection.entity.active"));

			if(entityDetection)
			{
				Console.WriteLine("Start entity detection...");

				string sensorType=config.GetElementAsString("xml.detection.entity.sensortype");
				string networkSegment=config.GetElementAsString("xml.detection.entity.networksegment");
				int minimumCount=Convert.ToInt32(config.GetElementAsString("xml.detection.entity.minimumcount"));
				TimeSpan minimumTime=new TimeSpan(0, 0, Convert.ToInt32(config.GetElementAsString("xml.detection.entity.minimumtime")));

				long significantTimeAsLong=Convert.ToInt64(config.GetElementAsString("xml.detection.entity.significanttime"));
				DateTime significantTime=new DateTime(significantTimeAsLong);
				
				if(sensorType=="network")
				{
					Console.WriteLine("Start entity detection with sensor type network...");

					if(networkScans.ContainsKey(networkSegment))
					{
						List<NetworkEntry> segmentEntries=networkScans[networkSegment];

						//Check if minimum count is reached
						if(segmentEntries.Count>minimumCount)
						{
							if(significantTimeAsLong==0)
							{
								significantTime=DateTime.Now;
								config.WriteElement("xml.detection.entity.significanttime", significantTime.Ticks.ToString());
							}

							//Check if minimum time is reached
							if(DateTime.Now>significantTime+minimumTime)
							{
								countEntities=segmentEntries.Count;
							}
						}
						else
						{
							config.WriteElement("xml.detection.entity.significanttime", "0");
						}
					}
					else
					{
						Console.WriteLine("Network segment for detection not found.");
					}
				}
			}
			#endregion

			#region API
			Console.WriteLine("Transfer sensor data to API...");

			//Write Network informations into API Database
			int countDevices=0;

			foreach(KeyValuePair<string, List<NetworkEntry>> pair in networkScans)
			{
				countDevices+=pair.Value.Count;
				eAPI.SetStatus(pair.Key, pair.Value.Count);
			}

			eAPI.SetStatus("devices", countDevices);

			//Write Entites
			eAPI.SetStatus("entities", countEntities);
			#endregion

			//set last run
			config.WriteElement("xml.misc.lastrun", DateTime.Now.Ticks.ToString());
			config.Save();
        }
    }
}
