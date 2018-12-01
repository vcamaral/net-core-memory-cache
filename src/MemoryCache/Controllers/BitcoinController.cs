using MemoryCache.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace MemoryCache.Controllers
{
    [Route("api/bitcoin")]
    public class BitcoinController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetBitcoinPrice([FromServices] IHttpClientFactory httpClientFactory)
        {
            var client = httpClientFactory.CreateClient("MercadoBitcoin");
            var response = await client.GetStringAsync("api/BTC/ticker");
            var bitcoin = JsonConvert.DeserializeObject<Bitcoin>(response);

            return Ok(new { Price = bitcoin.Ticker.Buy });
        }
    }
}