using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<List<double>> Get()
        {
            var connString = "Host=localhost;Username=postgres;Password=password";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            var list = new List<double>(10);
            var timer = new Stopwatch();
            for (int i = 0; i < 10; i++)
            {
                timer.Restart();
                await using (var cmd = new NpgsqlCommand("SELECT 1", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                        reader.GetInt32(0);
                timer.Stop();
                list.Add(timer.Elapsed.TotalMilliseconds);
            }
            return list;
        }
    }
}
