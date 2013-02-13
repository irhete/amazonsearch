using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amazon.Models
{
    public class SearchResultModel
    {
        private List<Item> items;
        private int totalPages;
        private int totalResults;
        private double time;

        public SearchResultModel(List<Item> items, double time)
        {
            this.items = items;
            this.time = time;
        }

        public SearchResultModel(List<Item> items, int totalPages, int totalResults, double time)
        {
            this.items = items;
            this.totalPages = totalPages;
            this.totalResults = totalResults;
            this.time = time;
        }

        public List<Item> GetItems()
        {
            return items;
        }
        public int GetTotalPages()
        {
            return totalPages;
        }
        public int GetTotalResults()
        {
            return totalResults;
        }
        public double GetTime()
        {
            return time;
        }
    }
}