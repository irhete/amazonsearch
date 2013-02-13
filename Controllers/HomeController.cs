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
            return View("Index"); 
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
            ViewData["totalResults"] = result.getTotalResults();
            ViewData["time"] = String.Format(new System.Globalization.CultureInfo("en-GB"), "{0:0.0000}", result.getTime());
            return View();
            
        }

        
    }
    }
