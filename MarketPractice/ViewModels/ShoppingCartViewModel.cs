using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketPractice.ViewModels
{
    public class ShoppingCartViewModel
    {
        public string itemId { get; set; }
        public string itemName { get; set; }
        public string imgUrl { get; set; }
        public int amount { get; set; }
    }
}