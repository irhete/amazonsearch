﻿using System;
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

            SearchResultModel result = ItemsFromAmazon.GetAmazonInfo(keywords, page);

            ViewData["items"] = result.GetItems();
            ViewData["totalPages"] = result.GetTotalPages();
            ViewData["totalResults"] = result.GetTotalResults();
            ViewData["time"] = String.Format(new System.Globalization.CultureInfo("en-GB"), "{0:0.0000}", result.GetTime());
            return View();

        }


    }
}
