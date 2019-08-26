using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CSStat.CsLogsApi.Interfaces;
using Ninject;

namespace CSStat.WebApp.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver()
        {
            _kernel = new StandardKernel();
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<ICsLogsApi>().To<CsStat.LogApi.CsLogsApi>();
        }
    }
}