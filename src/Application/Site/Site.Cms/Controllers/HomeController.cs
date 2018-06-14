using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MicBeach.Develop.CQuery;
using MicBeach.Util.Drawing;
using MicBeach.Web.Utility;
using MicBeach.Web.Mvc;
using TestWeb.Controllers.Base;
using MicBeach.Util;
using MicBeach.Util.Response;

namespace TestWeb.Controllers
{
    public class HomeController : WebBaseController
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            return View("Index");
        }
    }
}