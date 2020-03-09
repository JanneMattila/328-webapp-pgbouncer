using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Diagnostics;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueryController : ControllerBase
    {
        private readonly ILogger<QueryController> _logger;

        public QueryController(ILogger<QueryController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<QueryResponse> Post(QueryRequest request)
        {
            var response = new QueryResponse();
            await using var conn = new NpgsqlConnection(request.ConnectionString);
            await conn.OpenAsync();

            var timer = new Stopwatch();
            for (int i = 0; i < request.Count; i++)
            {
                timer.Restart();
                await using (var cmd = new NpgsqlCommand(request.Query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                        reader.GetInt32(0);
                timer.Stop();
                response.Durations.Add(timer.Elapsed.TotalMilliseconds);
            }
            return response;
        }
    }
}
