using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Net;
using System.Xml;
using AmazonProductAdvtApi;
using System.Diagnostics;
using System.Globalization;

namespace Amazon.Models
{
    public class ItemsFromAmazon
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
            System.Console.WriteLine("Start.");
            string requestUrl;
            Signer signer = new Signer(AWS_ACCESS_KEY_ID, AWS_SECRET_KEY, DESTINATION);

            string[] words = keywords.Split(' ');
            keywords = "";
            foreach (string word in words)
            {
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
            SearchResultModel result = FetchItemsInfo(requestUrl);
            List<Item> items = result.getItems();

            List<Item> newItems = new List<Item>();
            int i = (page - 1) * 13;
            while (i < items.Count && i < page * 13)
            {
                newItems.Add(items[i]);
                i++;
            }
            if (i <= items.Count)
            {
            }
            int totalPages = (int)Math.Ceiling(items.Count / 13.0);

            return new SearchResultModel(newItems, totalPages, items.Count, result.getTime());
        }


        private static SearchResultModel FetchItemsInfo(string url)
        {
            List<Item> items = new List<Item>();
            double time = 0;
            try
            {
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                XmlDocument doc = new XmlDocument();
                doc.Load(response.GetResponseStream());
                XmlNodeList itemNodes = doc.GetElementsByTagName("Item");

                XmlNodeList errorMessageNodes = doc.GetElementsByTagName("Message");
                if (errorMessageNodes != null && errorMessageNodes.Count > 0)
                {
                    String message = errorMessageNodes.Item(0).InnerText;
                    throw new Exception("Error: " + message + " (but signature worked)");

                }


                XmlNodeList timeNodes = doc.GetElementsByTagName("RequestProcessingTime");
                time = Double.Parse(timeNodes[0].InnerText, CultureInfo.InvariantCulture);


                foreach (XmlNode item in itemNodes)
                {
                    XmlElement elem = (XmlElement)(item);
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
                    catch
                    {
                        continue;
                    }

                    try
                    {
                        price = Convert.ToInt64(elem["ItemAttributes"]["ListPrice"]["Amount"].InnerText);

                    }
                    catch
                    {
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

            return new SearchResultModel(items, time);
        }
    }

}
    
