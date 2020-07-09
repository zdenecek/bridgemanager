using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Tools
{
    public static class ContainerConfig
    {

        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();



            return builder.Build();
        }
    }
}
