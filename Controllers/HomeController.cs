using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amazon.Models;
using System.Diagnostics;

namespace MvcDemo.Controllers
{
    public class HomeController : Controller
    {
       

        public ActionResult Index()
        {
            Dictionary<string, string> currencies = Currencies.getCurrencies();
            ViewData["currencies"] = currencies;
            return View(); 
        }


        [HttpGet]
        public ActionResult SearchResult(string keywords, int page)
        {
            
            if (keywords == "")
            {
                return Index();
            }

            SearchResultModel result = Items.GetAmazonInfo(keywords, page);

            ViewData["items"] = result.getItems();
            ViewData["totalPages"] = result.getTotalPages();
            return View();
            
        }

        
    }
    }
