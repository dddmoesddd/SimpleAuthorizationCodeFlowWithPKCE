// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SimpleAuthorizationCodeFlowWithPKCE;
using System.Net;

var builder = new HostBuilder()
          .ConfigureServices((hostContext, services) =>
          {
              // with AddHttpClient we register the IHttpClientFactory
              services.AddHttpClient();
              // here, we register the dependency injection  
              services.AddTransient<IInitialConection, InitialConection>();
          }).UseConsoleLifetime(); ;




var host = builder.Build();
var myService = host.Services.GetRequiredService<IInitialConection>();
var obj = await myService.GetCode();

//Console.ReadKey();

