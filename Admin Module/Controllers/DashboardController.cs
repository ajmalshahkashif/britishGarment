﻿using Microsoft.AspNetCore.Mvc;

namespace Admin_Module.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {

            return View();
        }
    }
}
