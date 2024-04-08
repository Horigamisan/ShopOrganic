using System.Data.Entity;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;
using WebDemo.Controllers;
using WebDemo.Models;
using WebDemo.Services.Implementations;
using WebDemo.Services.Interfaces;

namespace WebDemo.App_Start
{
    public class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // đăng ký tất cả các components của bạn tại đây
            // nó sẽ tự động đăng ký các interface với implementation mỗi khi bạn thêm bất kỳ service nào mới
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<IFavoriteService, FavoriteService>();
            container.RegisterType<ILayoutService, LayoutService>();
            container.RegisterType<ICartService, CartService>();
            container.RegisterType<IOrdersService, OrdersService>();
            container.RegisterType<IBlogService, BlogService>();
            
            container.RegisterType<DbContext, ShopOnlineEntities>(new PerRequestLifetimeManager());

            container.RegisterType<AccountController>(new InjectionConstructor());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}