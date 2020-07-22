using Microsoft.Extensions.DependencyInjection;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;

namespace XFBluetoothPrinter
{
    public class ShinyAppStartup : Shiny.ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.UseBleCentral();
            services.UseBlePeripherals();
        }
    }
}
