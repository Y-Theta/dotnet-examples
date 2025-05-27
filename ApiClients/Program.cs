using ApiClients.Fireflyiii;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

using WebApiClientCore;
using WebApiClientCore.Implementations;
using WebApiClientCore.Internals;

namespace ApiClients
{
    public class Program
    {
        private static string _token = null;

        public static void Main()
        {
            _token = Environment.GetEnvironmentVariable("FIREFLY_TOKEN");
            var services = new ServiceCollection();
            var builder = services
                .AddHttpApi<IFireFlyClient>()
                .ConfigureHttpApi(o =>
                {
                    o.HttpHost = new Uri("https://bill.y-theta.cn/api/", UriKind.Absolute);
                    o.GlobalFilters.Add(new DApiFilter());
                });
            var provider = services.BuildServiceProvider();
            var client = provider.GetService<IFireFlyClient>();

            var guid = Guid.NewGuid();
            var s = guid.ToString();
            var str = client.ListTransactions(guid,
                    start: new DateTime(2023, 6, 20),
                    end: new DateTime(2024, 7, 30)).GetAwaiter().GetResult();
            var trans = str.data.SelectMany(d => d.attributes.transactions.Select(t=>t.category_name));
            //var str = client.ListAccountAsync(null, 20, 0, null, AccountTypeFilter.All).GetAwaiter().GetResult();
        }


        public class DApiFilter : IApiFilter
        {
            public async Task OnRequestAsync(ApiRequestContext context)
            {
                context.HttpContext.RequestMessage.Headers.Add("Authorization", $"Bearer {_token}");
                await Task.CompletedTask;
            }

            public async Task OnResponseAsync(ApiResponseContext context)
            {
                await Task.CompletedTask;
            }
        }

        public class DHttpApiInterceptor : IHttpApiInterceptor
        {
            public object Intercept(ApiActionInvoker actionInvoker, object?[] arguments)
            {
                return default;
            }
        }
    }
}
