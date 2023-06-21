using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppLibrary
{
    public class Initializer
    {
        public static void Init()
        {

            IServiceProvider serviceProvider = BuildServiceProvider();

            var myService = serviceProvider.GetRequiredService<IMyService>();


#if NET5_0_OR_GREATER

            Logger.LogInfo($"Before calling NativeMethods.GetNativeHostData");

            var nativeHostData = NativeMethods.GetNativeHostData();

            Logger.LogInfo($"After calling NativeMethods.GetNativeHostData");
#else
            Logger.LogInfo("Not supported in current TFM");
#endif

        }



        private static IServiceProvider BuildServiceProvider()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> {
                    { "Name","foobar" }
                });
            IConfiguration config = configurationBuilder.Build();

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<IMyService, MyService>()
                .AddSingleton<IConfiguration>(config)
                .AddLogging(c => { c.AddConsole(); })
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
