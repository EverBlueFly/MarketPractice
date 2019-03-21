using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MarketPractice.Entity;

namespace MarketPractice.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            string sTest;
            return View();
        }
        public ActionResult LogIn()
        {
                return View();
        }
        [HttpPost]
        public ActionResult LogIn(UserInfo userit)
        {
            if(userit != null && !string.IsNullOrEmpty(userit.UserId))
            {
                using(MarketDBEntities db = new MarketDBEntities())
                {
                    var UserDs = (from us in db.UserInfo
                                  where us.UserId == userit.UserId
                                  select us).FirstOrDefault();
                    if(UserDs == null)
                    {
                        return View();
                    }
                    Session["UserId"] = userit.UserId;
                }
            }
            else
            {
                //應該要SHOW錯誤
                return View();
            }
            return View("Index");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserInfo userit)
        {
            if(userit != null && !string.IsNullOrEmpty(userit.UserId)  && !string.IsNullOrEmpty(userit.UserName))
            {
                using(MarketDBEntities db = new MarketDBEntities())
                {
                    var UserDs = (from us in db.UserInfo
                                  where us.UserId == userit.UserId
                                  select us).FirstOrDefault();
                    if(UserDs != null)
                    {
                        ViewBag.Errmsg = "使用者已存在!";
                        return View("Register");
                    }
                    else
                    {
                        UserInfo InsUserInfo = new UserInfo();
                        InsUserInfo.UserId = userit.UserId;
                        InsUserInfo.UserName = userit.UserName;

                        db.UserInfo.Add(InsUserInfo);
                        db.SaveChanges();
                        Session["UserId"] = userit.UserId;
                    }

                }
            }
            else
            {
                ViewBag.Errmsg = "請輸入有效的使用者資料";
                return View();
            }
            return View("Index");
        }
        public ActionResult LogOut()
        {
            Session.Remove("UserId");
            return RedirectToAction("Index");
        }
    }
}