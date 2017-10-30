using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWall.Models;
using Microsoft.AspNetCore.Http;


namespace TheWall.Controllers
{
   public class LogRegController : Controller
    {
        private readonly DbConnector _dbConnector;
        public LogRegController(DbConnector connect)
        {
            _dbConnector = connect;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if ((string)HttpContext.Session.GetString("userEmail") != null)
            {
                return RedirectToAction("Index", "Main");
            }
            RegModel reg = new RegModel();
            LogModel log = new LogModel();
            ViewBag.reg = reg;
            ViewBag.log = log;

            if (TempData["loginError"] != null)
            {
                ViewBag.loginError = TempData["loginError"];
            }
            return View("Index", reg);
        }
        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegModel model)
        {
            Console.WriteLine(ModelState.IsValid);
            if (ModelState.IsValid)
            {
                _dbConnector.Execute(string.Format("INSERT INTO Users (firstName, lastName, email, password, created_at) VALUES ('{0}', '{1}', '{2}', '{3}', NOW())", model.firstName, model.lastName, model.email, model.password));
                HttpContext.Session.SetString("userEmail", model.email);
                return RedirectToAction("Index", "Main");
            }
            return View(model);
            
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LogModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _dbConnector.Query(string.Format("SELECT * FROM Users WHERE email = '{0}'", model.email));
                Console.WriteLine(user.Count);
                if (user.Count < 1)
                {
                    TempData["loginError"] = "No user with that email found";
                    return RedirectToAction("Index");
                }
                else{
                    if ((string)user[0]["password"] == model.password)
                    {
                        HttpContext.Session.SetString("userEmail", model.email);
                        return RedirectToAction("Index", "Main");
                    }
                    else
                    {
                        TempData["loginError"] = "That password does not match what is on file";
                        return RedirectToAction("Index");
                    }

                }
            }
            return View(model);
        }

        
    }
}
