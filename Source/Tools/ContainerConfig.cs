using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Tools
{
    public static class ContainerConfig
    {

        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.Load(nameof(BridgeManager)))
                .Where(t => t.Name.Contains("ViewModel")).AsSelf().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.Load(nameof(BridgeManager)))
                .Where(t => t.Name.EndsWith("Service"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name.Contains(t.Name)));

            return builder.Build();
        }
    }
}
