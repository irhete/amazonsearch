using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Diagnostics;

namespace Amazon.Models
{
    public class Currencies
    {
        public static Dictionary<string, string> getCurrencies() {
            string url = "http://openexchangerates.org/api/currencies.json?app_id=83199c2d40c34d21842aa05702e1cadc";
            
            try{
                WebRequest request = HttpWebRequest.Create(url);
                request.Method = "GET";

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string objText = reader.ReadToEnd();
                return (new JavaScriptSerializer()).Deserialize<Dictionary<string, string>>(objText);
            }
                catch (Exception e)
            {
                Debug.WriteLine("Caught Exception: " + e.Message);
                Debug.WriteLine("Stack Trace: " + e.StackTrace);
            }
            return new Dictionary<string,string>();
        
    }
    }
}