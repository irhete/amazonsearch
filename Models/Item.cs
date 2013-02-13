using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amazon.Models
{
    public class Item
    {
        string title { get; set; }
        long price { get; set; }
        string image { get; set; }
        string url { get; set; }

        public Item(string title, long price, string image, string url)
        {
            this.title = title;
            this.price = price;
            this.image = image;
            this.url = url;
        }

        public string getTitle()
        {
            return title;
        }

        public long getPrice()
        {
            return price;
        }

        public string getImage()
        {
            return image;
        }

        public string getUrl()
        {
            return url;
        }
    }
}