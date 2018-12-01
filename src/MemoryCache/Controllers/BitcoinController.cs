using MemoryCache.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MemoryCache.Controllers
{
    [Route("api/bitcoin")]
    public class BitcoinController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetBitcoinPrice(
            [FromServices] IHttpClientFactory httpClientFactory,
            [FromServices] IOptions<CacheSettings> cacheSettings,
            [FromServices] IMemoryCache memoryCache)
        {
            var bitcoinPrice = await memoryCache.GetOrCreate("BitcoinPrice", async cache =>
            {
                cache.SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheSettings.Value.ExpirationInSeconds));

                var client = httpClientFactory.CreateClient("MercadoBitcoin");
                var response = await client.GetStringAsync("api/BTC/ticker");
                var bitcoin = JsonConvert.DeserializeObject<Bitcoin>(response);

                return bitcoin.Ticker.Buy;
            });
            

            return Ok(new { Price = bitcoinPrice });
        }
    }
}