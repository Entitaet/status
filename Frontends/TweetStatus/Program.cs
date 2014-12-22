using System;
using System.Collections.Generic;
using CSCL;
using System.IO;
using System.Net;
using System.Data;
using CSCL.Network.REST;
using TinyTwitter;

namespace TweetStatus
{
    class Program
    {
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

            string apiToken=config.GetElementAsString("xml.api.token");
            string apiUrl=config.GetElementAsString("xml.api.url");

            string twitterConsumerKey=config.GetElementAsString("xml.twitter.consumerkey");
            string twitterConsumerSecret=config.GetElementAsString("xml.twitter.consumersecret");

            string twitterAccessToken=config.GetElementAsString("xml.twitter.accesstoken");
            string twitterAccessTokenSecret=config.GetElementAsString("xml.twitter.accesstokensecret");

            //Create twitter token
            OAuthInfo token=null;

            if(twitterAccessToken==""||twitterAccessTokenSecret=="")
            {
                Console.WriteLine("Set access token in config file");
                return;
            }
            else
            {
                token=new OAuthInfo {
                    AccessToken=twitterAccessToken,
                    AccessSecret=twitterAccessTokenSecret,
                    ConsumerKey=twitterConsumerKey,
                    ConsumerSecret=twitterConsumerSecret
                };
            }

            //Check status database
            Console.WriteLine("Check status database");

            //Database
            RestClient client=new RestClient(apiUrl);
            string parameters=String.Format("entities/?token={0}", apiToken);

            string value=client.Request(parameters);
            int entityCount=Convert.ToInt32(value);

            Console.WriteLine("Entity count from api: {0}", entityCount);

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
                //Tweet
                DateTime now=DateTime.Now;

                string statusGreen=String.Format("Der Hackerspace ist besetzt ({0}:{1:##} Uhr) und kann besucht werden. #status", now.Hour, now.Minute);
                string statusYellow="";
                string statusRed=String.Format("Der Hackerspace ist nicht mehr besetzt ({0}:{1:##} Uhr).  #status", now.Hour, now.Minute);

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

                bool success=true;

                try
                {
                    Console.WriteLine("Token: {0}", token.ToString());

                    var twitter=new TinyTwitter.TinyTwitter(token);
                    twitter.UpdateStatus(tweetText);
                }
                catch(Exception ex)
                {
                    success=false;
                }

                if(success)
                {
                    //Write success on console
                    Console.WriteLine("Tweet sended: {0}", tweetText);

                    //Write new status
                    File.WriteAllText(entryFile, newStatus.ToString());
                }
                else
                {
                    Console.WriteLine("Tweet not sended: {0}", tweetText);
                }
            }
        }
    }
}
