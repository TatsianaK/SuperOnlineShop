using System.Web.Mvc;
using Microsoft.Practices.Unity;
using SuperOnlineShop.Helpers;
using Unity.Mvc3;

namespace SuperOnlineShop
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            ControllerBuilder.Current.SetControllerFactory(
                    new UnityControllerFactory(container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();            
            container.RegisterType<IShoppingCartRepository, ShoppingCartRepositoryTest>();
            container.RegisterType<IShoppingCart, ShoppingCart>();

            return container;
        }
    }
}