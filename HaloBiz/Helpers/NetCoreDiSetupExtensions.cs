//using HaloBiz.SMOServices;
using HaloBiz.MyServices;
using HaloBiz.MyServices.Impl;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Helpers
{
    public static class NetCoreDiSetupExtensions
    {
        public static void RegisterServiceLayerDi
            (this IServiceCollection services)
        {
            services.RegisterAssemblyPublicNonGenericClasses()
                .Where(x => x.Name.EndsWith("RepositoryImpl") 
                    || x.Name.EndsWith("Repository")
                    || x.Name.EndsWith("ServiceImpl")
                    || x.Name.EndsWith("ServiceImplV2")
                    || x.Name.EndsWith("Service")
                    //| x.Name.EndsWith("Services")
                    || x.Name.EndsWith("Adapter"))
                .Where(x => x.Name != "LeadConversionServiceImpl")
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            //put any non-standard DI registration, e.g. generic types, here
            //services.AddScoped<ISMORouteAndRegion, SMORoutesAndRegionServices>();
        }
    }
}
