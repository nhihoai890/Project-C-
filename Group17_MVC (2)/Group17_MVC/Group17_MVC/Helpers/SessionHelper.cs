using Group17_MVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group17_MVC.Helpers
{
    public class SessionHelper
    {
        public static List<CartItem> GetCart()
        {
            var cart = HttpContext.Current.Session["Cart"] as List<CartItem>;
            return cart ?? new List<CartItem>();
        }
        public static void AddToCart(CartItem item, int quantity)
        {
            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(i=> i.MaSach == item.MaSach);
            if (existingItem != null)
            {
                existingItem.SoLuong += quantity; 
            }
            else
            {
                item.SoLuong = quantity;
                cart.Add(item);
            }
            HttpContext.Current.Session["Cart"] = cart;

        }
        public static void RemoveFromCart(string maSach)
        {
            var cart = GetCart();
            var itemToRemove = cart.FirstOrDefault(i => i.MaSach == maSach);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Current.Session["Cart"] = cart;
            }
        }

        internal static void SetCart(List<CartItem> cart)
        {
            HttpContext.Current.Session["Cart"] = cart;
        }

        internal static void ClearCart()
        {
            HttpContext.Current.Session["Cart"] = new List<CartItem>();
        }
    }
}