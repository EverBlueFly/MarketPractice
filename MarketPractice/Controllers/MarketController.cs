using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MarketPractice.Entity;

namespace MarketPractice.Controllers
{
    public class MarketController : Controller
    {
        // GET: Market
        public ActionResult Index(string s = "1")
        {
            using (MarketDBEntities db = new MarketDBEntities())
            {
                var DS = (from pl in db.ProductList
                          select pl);
                return View(DS.ToList());
            }
        }
        public ActionResult AddCart(string itemId)
        {
            try
            {
                using (MarketDBEntities db = new MarketDBEntities())
                {
                    
                if (!string.IsNullOrEmpty(itemId))
                {
                        //string sUserId = "KrisIdoit";
                        if(Session["UserId"] == null) {
                            return RedirectToAction("Index");
                        }
                        string sUserId = Session["UserId"].ToString();
                        if (string.IsNullOrEmpty(sUserId))
                    {
                            return RedirectToAction("Index");
                        }
                    
                        var itDtl = (from pl in db.ProductList
                                     where pl.itemid == itemId
                                     select pl).FirstOrDefault();
                        if (itDtl.left_amount != 0)
                        {
                            itDtl.left_amount = itDtl.left_amount - 1;

                            var spit = (from sp in db.ShoppingList
                                        where sp.itemId == itemId && sp.UserId == sUserId
                                        select sp).FirstOrDefault();
                            if (spit != null)
                            {
                                spit.amount = spit.amount + 1;
                            }
                            else
                            {
                                Entity.ShoppingList insToSp = new ShoppingList();
                                insToSp.itemId = itemId;
                                insToSp.UserId = sUserId;
                                insToSp.amount = 1;
                                db.ShoppingList.Add(insToSp);
                            }
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}