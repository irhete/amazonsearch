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
            string requestUrl;
            Signer signer = new Signer(AWS_ACCESS_KEY_ID, AWS_SECRET_KEY, DESTINATION);

            keywords = EscapeKeywordString(keywords);

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

            List<Item> allItems = result.GetItems();

            List<Item> currentPageItems = FetchItemsForCurrentPage(page, allItems);

            int totalPages = (int)Math.Ceiling(allItems.Count / 13.0);

            return new SearchResultModel(currentPageItems, totalPages, allItems.Count, result.GetTime());
        }

        private static List<Item> FetchItemsForCurrentPage(int page, List<Item> items)
        {
            List<Item> newItems = new List<Item>();
            int i = (page - 1) * 13;
            while (i < items.Count && i < page * 13)
            {
                newItems.Add(items[i]);
                i++;
            }
            return newItems;
        }

        private static string EscapeKeywordString(string keywords)
        {
            string[] words = keywords.Split(' ');
            keywords = "";
            foreach (string word in words)
            {
                keywords += word + "%20";
            }
            keywords = keywords.Substring(0, keywords.Length - 3);
            return keywords;
        }


        private static SearchResultModel FetchItemsInfo(string url)
        {
            List<Item> items = new List<Item>();
            double time = 0;
            string title;
            long price;
            string image;
            string itemUrl;

            try
            {
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                XmlDocument doc = new XmlDocument();
                doc.Load(response.GetResponseStream());

                CheckIfErrors(doc);

                XmlNodeList timeNodes = doc.GetElementsByTagName("RequestProcessingTime");
                time = Double.Parse(timeNodes[0].InnerText, CultureInfo.InvariantCulture);

                XmlNodeList itemNodes = doc.GetElementsByTagName("Item");
                foreach (XmlNode item in itemNodes)
                {
                    XmlElement elem = (XmlElement)(item);
                    try
                    {
                        title = elem["ItemAttributes"]["Title"].InnerText;
                        image = elem["SmallImage"]["URL"].InnerText;
                        itemUrl = elem["DetailPageURL"].InnerText;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Caught Exception: " + e.Message);
                        Debug.WriteLine("Stack Trace: " + e.StackTrace);
                        continue;
                    }

                    try
                    {
                        price = Convert.ToInt64(elem["ItemAttributes"]["ListPrice"]["Amount"].InnerText);
                    }
                    catch (Exception e)
                    {
                        price = 0;
                        Debug.WriteLine("Caught Exception: " + e.Message);
                        Debug.WriteLine("Stack Trace: " + e.StackTrace);
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

        private static void CheckIfErrors(XmlDocument doc)
        {
            XmlNodeList errorMessageNodes = doc.GetElementsByTagName("Message");
            if (errorMessageNodes != null && errorMessageNodes.Count > 0)
            {
                String message = errorMessageNodes.Item(0).InnerText;
                throw new Exception("Error: " + message + " (but signature worked)");
            }
        }
    }

}
    
