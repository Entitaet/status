using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using CSCL.Network.REST;

namespace Sensor
{
	public class EAPI
	{
		string URL;
		string Token;

		public EAPI(string url, string token)
		{
			URL=url;
			Token=token;
		}

		string PutStatus(string key, string value)
		{
			//Put Status
			RestClient client=new RestClient(URL, HttpMethod.PUT);
			string parameters=String.Format("{0}/{1}/?token={2}", key, value, Token);

			HttpStatusCode statusCode;
			return client.Request(out statusCode, parameters);
		}

		public string GetStatus(string key)
		{
			HttpStatusCode statusCode;
			return GetStatus(key, out statusCode);
		}

		public string GetStatus(string key, out HttpStatusCode statusCode)
		{
			RestClient client=new RestClient(URL);
			string parameters=String.Format("{0}/?token={1}", key, Token);
			return client.Request(out statusCode, parameters);
		}

		public int GetStatusAsInt(string key)
		{
			string val=GetStatus(key);

			try
			{
				return Convert.ToInt32(val);
			}
			catch
			{
				return 0;
			}
		}

		public string SetStatus(string key, string value)
		{
			//Check if key exist
			HttpStatusCode statusCode;
			string keyStatus=GetStatus(key, out statusCode);

			switch(statusCode)
			{
				case HttpStatusCode.NoContent:
					{
						//Schlüssel existiert noch nicht und muss angelegt werden
						return PutStatus(key, value);
					}
				default:
					{
						//Statuscode ignoreren
						break;
					}
			}

			//POST Status
			RestClient client=new RestClient(URL, HttpMethod.POST);
			string parameters=String.Format("{0}/{1}/?token={2}", key, value, Token);
			return client.Request(parameters);
		}

		public string SetStatus(string key, int value)
		{
			return SetStatus(key, value.ToString());
		}
	}
}
