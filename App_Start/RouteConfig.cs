﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebDemo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Shop", "{type}/{meta}",
                new { controller = "Shop", action = "Index", meta = UrlParameter.Optional },
                new RouteValueDictionary(new { type = "cua-hang" }),
                namespaces: new[] { "WebDemo.Controllers" });

            routes.MapRoute("Home", "{type}/{meta}",
                new { controller = "Default", action = "Index", meta = UrlParameter.Optional },
                new RouteValueDictionary(new { type = "trang-chu" }),
                namespaces: new[] { "WebDemo.Controllers" });

            routes.MapRoute("OrderHistory", "{type}/{meta}",
                new { controller = "Checkout", action = "OrderHistory", meta = UrlParameter.Optional },
                new RouteValueDictionary(new { type = "lich-su-mua-hang" }),
                namespaces: new[] { "WebDemo.Controllers" });

            routes.MapRoute("Checkout", "{type}/{meta}",
                new { controller = "Checkout", action = "Index", meta = UrlParameter.Optional },
                new RouteValueDictionary(new { type = "thanh-toan" }),
                namespaces: new[] { "WebDemo.Controllers" });

            routes.MapRoute("StripeWebhook", "{type}",
               new { controller = "Checkout", action = "StripeWebhook" },
               new RouteValueDictionary(new { type = "stripe-webhook" }),
               namespaces: new[] { "WebDemo.Controllers" });

            routes.MapRoute("Carts", "{type}/{meta}",
                new { controller = "Carts", action = "Index", meta = UrlParameter.Optional },
                new RouteValueDictionary(new { type = "gio-hang" }),
                namespaces: new[] { "WebDemo.Controllers" });

            routes.MapRoute("FavoriteItem", "{type}",
                new { controller = "Product", action = "GetFavoriteProduct", meta = UrlParameter.Optional },
                new RouteValueDictionary(new { type = "san-pham-yeu-thich" }),
                namespaces: new[] { "WebDemo.Controllers" });

            routes.MapRoute("Product", "{type}/{meta}",
                new { controller = "Product", action = "Index", meta = UrlParameter.Optional },
                new RouteValueDictionary(new { type = "san-pham" }),
                namespaces: new[] { "WebDemo.Controllers" });

            routes.MapRoute("Detail", "{type}/{meta}/{id}",
               new { controller = "Product", action = "GetDetailProduct", meta = UrlParameter.Optional },
               new RouteValueDictionary(new { type = "san-pham" }),
               namespaces: new[] { "WebDemo.Controllers" });


            routes.MapRoute("Contact", "{type}/{meta}",
               new { controller = "Contact", action = "Index", meta = UrlParameter.Optional },
               new RouteValueDictionary(new { type = "lien-he" }),
               namespaces: new[] { "WebDemo.Controllers" });

            routes.MapRoute("Blog", "{type}/{meta}",
               new { controller = "Blog", action = "Index", meta = UrlParameter.Optional },
               new RouteValueDictionary(new { type = "blog" }),
               namespaces: new[] { "WebDemo.Controllers" });

            routes.MapRoute("DetailBlog", "{type}/{meta}/{id}",
              new { controller = "Blog", action = "getDetailBlog", meta = UrlParameter.Optional },
              new RouteValueDictionary(new { type = "blog" }),
              namespaces: new[] { "WebDemo.Controllers" });

            routes.MapRoute(
                name: "Login",
                url: "tai-khoan/dang-nhap",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "Register",
                url: "tai-khoan/dang-ky",
                defaults: new { controller = "Account", action = "Register" }
            );

            routes.MapRoute(
                name: "Logout",
                url: "tai-khoan/dang-xuat",
                defaults: new { controller = "Account", action = "LogOut" }
            );

            routes.MapRoute(
                name: "ExternalLoginCallback",
                url: "tai-khoan/xac-nhan-OAuth",
                defaults: new { controller = "Account", action = "ExternalLoginCallback" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WebDemo.Controllers" }
            );
        }
    }
}
