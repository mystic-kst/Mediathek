using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MediathekMvc.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to MYSTIK!";

			return View();
		}

		public ActionResult About()
		{
			return View();
		}
	}
}
