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

        public SearchResultModel(List<Item> items, int totalPages) {
            this.items = items;
            this.totalPages = totalPages;
        }

        public List<Item> getItems() {
            return items;
        }
        public int getTotalPages() {
            return totalPages;
        }
    }
}