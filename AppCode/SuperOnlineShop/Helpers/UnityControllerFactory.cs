using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace SuperOnlineShop.Helpers {
    public class UnityControllerFactory : DefaultControllerFactory {

        IUnityContainer container;

        public UnityControllerFactory(IUnityContainer container) {
            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType) {
            try {
                    if (controllerType == null)
                        throw new ArgumentNullException("controllerType");
    
                    if (!typeof(IController).IsAssignableFrom(controllerType))
                        throw new ArgumentException(string.Format(
                            "Type requested is not a controller: {0}", controllerType.Name),
                            "controllerType");
                    return container.Resolve(controllerType) as IController;
                }
                catch { return null; }
        }
    }
}