using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MarketPractice.Entity;
using MarketPractice.ViewModels;

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
                    Session["UserId"] = UserDs.UserId;
                    Session["UserName"] = UserDs.UserName;
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
                        Session["UserName"] = userit.UserName;
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
            Session.Remove("UserName");
            return RedirectToAction("Index");
        }
        public ActionResult ShoppingCart()
        {
            if(Session["UserId"] != null)
            {
                string sUserId = Session["UserId"].ToString();
                using(MarketDBEntities db = new MarketDBEntities())
                {
                    var itDS = (from sl in db.ShoppingList
                                join pl in db.ProductList on sl.itemId equals pl.itemid
                                where sl.UserId == sUserId
                                select new ShoppingCartViewModel {
                                    itemId = sl.itemId,
                                    itemName = pl.itemName,
                                    amount = sl.amount,
                                    imgUrl = pl.imgUrl
                                }).ToList();
                    return View(itDS);
                }
            }
            return RedirectToAction("Index","Home");
        }
        public ActionResult DeleteFromCart(string itemId)
        {
            if(Session["UserId"] != null && !string.IsNullOrEmpty(itemId))
            {
                string sUserId = Session["UserId"].ToString();
                using (MarketDBEntities db = new MarketDBEntities())
                {
                    var SPDS = (from sp in db.ShoppingList
                                where sp.itemId == itemId &&
                                sp.UserId == sUserId
                                select sp).FirstOrDefault();
                    if(SPDS != null)
                    {
                        var PDDS = (from pl in db.ProductList
                                    where pl.itemid == itemId
                                    select pl).FirstOrDefault();
                        if(PDDS != null)
                        {
                            PDDS.left_amount = PDDS.left_amount + SPDS.amount;
                        }
                        db.ShoppingList.Remove(SPDS);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("ShoppingCart");
        }
        //public ActionResult GetPic(string sFileName)
        //{
        //    string sPath = Server.MapPath(".");
        //    return File(sPath + "/imgs/" + sFileName, "image/jpeg");
        //}
    }
}