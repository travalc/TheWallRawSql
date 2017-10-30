using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWall.Models;
using Microsoft.AspNetCore.Http;

namespace TheWall.Controllers
{
    public class MainController : Controller
    {
        private readonly DbConnector _dbConnector;
        public MainController(DbConnector connect)
        {
            _dbConnector = connect;
        }
        [HttpGet]
        [Route("Main")]
        public IActionResult Index()
        {
            string email = (string)HttpContext.Session.GetString("userEmail");
            if (email == null)
            {
                return RedirectToAction("Index", "LogReg");
            }
            var user = _dbConnector.Query(string.Format("SELECT * FROM Users WHERE email = '{0}'", email));
            int userId = (int)user[0]["id"];
            HttpContext.Session.SetString("userId", userId.ToString());
            MessageModel message = new MessageModel();
            CommentModel comment = new CommentModel();
            ViewBag.message = message;
            ViewBag.comment = comment;

            var messageQuery = _dbConnector.Query("SELECT * FROM Messages ORDER BY created_at DESC").ToList();
            List<Dictionary<string,object>> messages = new List<Dictionary<string,object>>();
            foreach (var item in messageQuery)
            {
                var messageUser = _dbConnector.Query(string.Format("SELECT * FROM USERS WHERE id = '{0}'", item["Users_id"]));
                var commentQuery = _dbConnector.Query(string.Format("SELECT * FROM Comments WHERE Messages_id = '{0}'", item["id"]));
                List<Dictionary<string,object>> comments = new List<Dictionary<string,object>>();
                foreach (var com in commentQuery)
                {
                    var commentUser = _dbConnector.Query(string.Format("SELECT * FROM USERS WHERE id ='{0}'", com["Users_id"]));
                    var commentObj = new Dictionary<string,object>() {
                        {"comment", com["comment"]},
                        {"user", string.Format("{0}, {1}", (string)commentUser[0]["firstName"], (string)commentUser[0]["lastName"])},
                        {"date", com["created_at"]}
                    };
                    comments.Add(commentObj);
                }
                var obj = new Dictionary<string, object>() {
                    {"message", item["message"].ToString()},
                    {"user", string.Format("{0} {1}", (string)messageUser[0]["firstName"], (string)messageUser[0]["lastName"])},
                    {"date", item["created_at"]},
                    {"id", item["id"]},
                    {"comments", comments}
                };
                messages.Add(obj);
            }
            ViewBag.messages = messages;
            foreach(var m in messages)
            {
                Console.WriteLine(m["date"]);
            }
            return View();
        }

        [HttpPost]
        [Route("Logout")]

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "LogReg");
        }

        [HttpPost]
        [Route("Message")]

        public IActionResult Message(MessageModel model)
        {
            if (ModelState.IsValid)
            {
                string id = HttpContext.Session.GetString("userId");
                _dbConnector.Execute(string.Format("INSERT INTO Messages (message, Users_id, created_at, updated_at) VALUES('{0}', '{1}', NOW(), NOW())", model.message, id));
                return RedirectToAction("Index");           
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [Route("Comment")]

        public IActionResult Comment(CommentModel model)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine(model.Messages_id);
                string userId = HttpContext.Session.GetString("userId");
                _dbConnector.Execute(string.Format("INSERT INTO Comments (comment, Users_id, Messages_id, created_at, updated_at) VALUES('{0}', '{1}', '{2}', NOW(), NOW())", model.comment, userId, model.Messages_id));
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
    }
}