using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ApiClients.Fireflyiii
{
    [LoggingFilter]
    [JsonReturn(EnsureMatchAcceptContentType = false)]
    public interface IFireFlyClient : IHttpApi
    {
        [HttpGet("v1/about")]
        ITask<string> About();

        [HttpGet("v1/accounts")]
        ITask<string> ListAccounts();

        [HttpGet("v1/transactions")]
        ITask<FireflyTransactionResponse> ListTransactions(
            [Header("X-Trace-Id")] Guid? traceid = null, 
            int? limit = null, 
            int? page = null, 
            DateTime? start = null,
            DateTime? end = null, 
            CancellationToken token = default);

        [HttpGet("v1/categories")]
        ITask<FireflyCategoryResponse> ListCategories(
            [Header("X-Trace-Id")] Guid? traceid = null,
            int? limit = null,
            int? page = null,
            CancellationToken token = default);

        [HttpGet("v1/bills")]
        ITask<string> ListBills();
    }
}
