﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gswsBackendAPI.Controllers
{
    public class RevenueController : Controller
    {
        // GET: Revenue
        public ActionResult IncomeCertificate()
        {
            return View();
        }
    }
}