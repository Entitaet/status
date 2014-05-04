using System;
using System.Collections.Generic;
using CSCL;
using System.IO;
using TweetinCore.Interfaces;
using Tweetinvi;
using TweetinCore.Interfaces.TwitterToken;
using TwitterToken;
using TweetinCore.Events;
using System.Net;
using CSCL.Database.MySQL;
using System.Data;

namespace TweetStatus
{
    class Program
    {
   		static IToken GenerateToken(string consumerKey, string consumerSecret, RetrieveCaptchaDelegate getCaptchaDelegate)
		{
			Console.WriteLine("Starting Token Generation...");
			ITokenCreator creator = new TokenCreator(consumerKey, consumerSecret);

			Console.WriteLine("Please enter the verifier key...");
			IToken newToken = creator.CreateToken(getCaptchaDelegate);

			if (newToken != null)
			{
				Console.WriteLine("Token generated!");
				Console.WriteLine("Token Information : ");

				Console.WriteLine("Consumer Key : {0}", newToken.ConsumerKey);
				Console.WriteLine("Consumer Secret : {0}", newToken.ConsumerSecret);
				Console.WriteLine("Access Token : {0}", newToken.AccessToken);
				Console.WriteLine("Access Token Secret : {0}", newToken.AccessTokenSecret);

				ITokenUser loggedUser = new TokenUser(newToken);
				Console.WriteLine("Your name is {0}!", loggedUser.ScreenName);

				return newToken;
			}

			Console.WriteLine("Token could not be generated. Please login and specify your verifier key!");
			return null;
		}

		static int GetCaptchaFromConsole(string validationUrl)
		{
			Console.WriteLine("Please visit :");
			Console.WriteLine("{0}", validationUrl);
			Console.WriteLine("\nEnter validation key : ");
			string validationKey = Console.ReadLine();

			int result;
			if (Int32.TryParse(validationKey, out result))
			{
				return result;
			}

			return -1;
		}

        public static void Main(string[] args)
        {
            //Load config
            XmlData config;

            try
            {
                config=new XmlData("tweetstatus.xml");
            }
            catch(Exception e)
            {
                Console.WriteLine("Konfiguration konnte nicht gelesen werden.");
                Console.WriteLine(e.ToString());
                return;
            }

            string miscCheckCertificates=config.GetElementAsString("xml.misc.checkcertificates");

            if(miscCheckCertificates.ToLower()=="false")
            {
                //Disable certificate check
                ServicePointManager.ServerCertificateValidationCallback=delegate
                {
                    return true;
                };
            }

            string databaseServer=config.GetElementAsString("xml.database.server");
            int databasePort=Convert.ToInt32(config.GetElementAsString("xml.database.port"));
            string databaseName=config.GetElementAsString("xml.database.database");
            string databaseUsername=config.GetElementAsString("xml.database.username");
            string databasePassword=config.GetElementAsString("xml.database.password");

            string twitterConsumerKey=config.GetElementAsString("xml.twitter.consumerkey");
            string twitterConsumerSecret=config.GetElementAsString("xml.twitter.consumersecret");

            string twitterAccessToken=config.GetElementAsString("xml.twitter.accesstoken");
            string twitterAccessTokenSecret=config.GetElementAsString("xml.twitter.accesstokensecret");

            //Create twitter token
            IToken token=null;

            if(twitterAccessToken==""||twitterAccessTokenSecret=="")
            {
                token=GenerateToken(twitterConsumerKey, twitterConsumerSecret, GetCaptchaFromConsole);
            }
            else
            {
                token=new Token(twitterAccessToken, twitterAccessTokenSecret, twitterConsumerKey, twitterConsumerSecret);
            }

            //Check status database
            Console.WriteLine("Check status database");

            //Database
            MySQL db=new MySQL(databaseServer, databasePort, databaseName, databaseUsername, databasePassword);

            bool dbConnected=db.Connect();

            int entityCount=-1;

            if(dbConnected)
            {
                string sql="SELECT * FROM status WHERE `key`='entities'";
                DataTable table=db.ExecuteQuery(sql);
                entityCount=Convert.ToInt32(table.Rows[0]["value"]);
                db.Disconnect();
            }
            else
            {
                Console.WriteLine("Database connection failed.");
                return;
            }

            if(entityCount==-1)
            {
                Console.WriteLine("No valid entity value.");
                return;
            }

            //Check status file and tweet if nessesary
            //Load known entries
            string entryFile="status.txt";

            Entry oldStatus=new Entry(0);

            if(File.Exists(entryFile))
            {
                oldStatus=new Entry(File.ReadAllLines(entryFile)[0]);
            }

            Entry newStatus=new Entry(entityCount);
           
            if(oldStatus!=newStatus)
            {
                //Write new status
                File.WriteAllText(entryFile, newStatus.ToString());

                //Tweet
                string statusGreen="Der Hackerspace ist besetzt und kann besucht werden. #status";
                string statusYellow="";
                string statusRed="Der Hackerspace ist nicht mehr besetzt. #status";

                string tweetText="";

                if(newStatus.EntityCount==0)
                {
                    tweetText=statusRed;
                }
                else
                {
                    if(oldStatus.EntityCount>0)
                    {
                        Console.WriteLine("Update not necessary.");
                        return;
                    }
                  
                    tweetText=statusGreen;
                }

                bool success=false;

                try
                {
                    ITweet tweet=new Tweet(tweetText, token);
                    success=tweet.Publish();
                }
                catch(Exception ex)
                {
                    if(ex is NullReferenceException)
                    {
                        //wahrscheinlich ein Fehler beim deserialisieren der Antwort
                        //kann ignoriert werden
                    }
                }

                if(success)
                {
                    //Write success on console
                    Console.WriteLine("Tweet sended: {0}", tweetText);
                }
                else Console.WriteLine("Tweet not sended: {0}", tweetText);
            }
        }
    }
}
