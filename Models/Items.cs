using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Net;
using System.Xml;
using AmazonProductAdvtApi;
using System.Diagnostics;

namespace Amazon.Models
{
    public class Items
    {

        private const string AWS_ACCESS_KEY_ID = "AKIAJDUGWPW4F5SSEN4A";
        private const string AWS_SECRET_KEY = "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";
        private const string DESTINATION = "ecs.amazonaws.com";
        private const string ASSOCIATE_TAG = "0545010225";

        private const string NAMESPACE = "http://webservices.amazon.com/AWSECommerceService/2011-08-01";
            
        private const string SERVICE = "AWSECommerceService";
        private const string VERSION = "2011-08-01";

        public static SearchResultModel GetAmazonInfo(string keywords, int page)
        {
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
            System.Console.WriteLine("Start.");
            string requestUrl;
            Signer signer = new Signer(AWS_ACCESS_KEY_ID, AWS_SECRET_KEY, DESTINATION);

            string[] words = keywords.Split(' ');
            keywords = "";
            foreach (string word in words) {
                keywords += word + "%20";
            }
            keywords = keywords.Substring(0, keywords.Length - 3);

           String requestString = "Service=" + SERVICE
               + "&Version=" + VERSION
               + "&Operation=ItemSearch"
               + "&ResponseGroup=Medium"
               + "&AssociateTag=" + ASSOCIATE_TAG
               + "&Keywords=" + keywords
               + "&SearchIndex=Blended"
               ;
            requestUrl = signer.Sign(requestString);
            List<Item> items =  FetchItemsInfo(requestUrl);
            Debug.WriteLine(requestUrl);

            List<Item> newItems = new List<Item>();
                int i = (page-1) * 13;
                while (i < items.Count && i < page * 13) { 
                    newItems.Add(items[i]);
                    i++;
                }
                if (i <= items.Count) { 
                }
                int totalPages = (int) Math.Ceiling(items.Count / 13.0);
            
            return new SearchResultModel(newItems, totalPages);
        }


            private static List<Item> FetchItemsInfo(string url)
        {
                List<Item> items = new List<Item>();
            try
            {
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                XmlDocument doc = new XmlDocument();
                doc.Load(response.GetResponseStream());

                XmlNodeList errorMessageNodes = doc.GetElementsByTagName("Message");
                if (errorMessageNodes != null && errorMessageNodes.Count > 0)
                {
                    String message = errorMessageNodes.Item(0).InnerText;
                    throw new Exception("Error: " + message + " (but signature worked)");
                    
                }

                XmlNodeList itemNodes = doc.GetElementsByTagName("Item");
                foreach (XmlNode item in itemNodes)
                {
                    XmlElement elem = (XmlElement) (item);
                    string title;
                    long price;
                    string image;
                    string itemUrl;
                    try
                    {
                        title = elem["ItemAttributes"]["Title"].InnerText;
                        image = elem["SmallImage"]["URL"].InnerText;
                        itemUrl = elem["DetailPageURL"].InnerText;
                    }
                    catch {
                        continue;
                    }
                    
                    try
                    {
                        price = Convert.ToInt64(elem["ItemAttributes"]["ListPrice"]["Amount"].InnerText);
                        Debug.WriteLine(price);
                    }
                    catch {
                        price = 0;
                    }
                    
                    items.Add(new Item(title, price, image, itemUrl));
                    
                }
                
                
            }
            catch (Exception e)
            {
                Debug.WriteLine("Caught Exception: " + e.Message);
                Debug.WriteLine("Stack Trace: " + e.StackTrace);
            }

            return items;
        }
    }
       
        }
    
