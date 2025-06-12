using System.Text.Json;
using technicaltest.Models;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace technicaltest.Repositories.Cache
{
	public interface IProductCache	{
	    Task<bool> SetCache(Product o);
	    Task<bool> SetCaches(List<Product> o);
	    Task<Product> GetCache(string id);
	    Task<List<Product>> GetCaches();
	    Task<bool> Clear();
	}

public class ProductCache : IProductCache
{
	private readonly IDistributedCache _cache;
	public ProductCache(IDistributedCache cache)
	{
	    _cache = cache ?? throw new ArgumentNullException(nameof(cache));
	}

	public async Task<bool> Clear()
	{
	    await _cache.RemoveAsync("l-Product-cache");
	    await _cache.RemoveAsync("Product.*");
	    return true;
	}

	public async Task<Product> GetCache(string id)
	{
	    try
	    {
	        var o = await _cache.GetStringAsync($"Product.{id}");
	        if (o != null)
	        {
	            return JsonConvert.DeserializeObject<Product>(o);
	        }
	        return null;
	    }
	    catch
	    {
	        throw;
	    }
	}

	public async Task<List<Product>> GetCaches()
	{
	    try
	    {
	        var o = await _cache.GetStringAsync($"l-Product-cache");
	        if (o != null)
	        {
	            var lCurr = JsonConvert.DeserializeObject<List<Product>>(o);
	            return lCurr;
	        }

	        return null;
	    }
	    catch
	    {
	        throw;
	    }
	}

	public async Task<bool> SetCache(Product o)
	{
	    await _cache.SetStringAsync($"Product.{o.Id}", JsonConvert.SerializeObject(o), new DistributedCacheEntryOptions
	    {
	        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
	    });
	    return true;
	}

	public async Task<bool> SetCaches(List<Product> o)
	{
	    await _cache.SetStringAsync($"l-Product-cache", JsonConvert.SerializeObject(o), new DistributedCacheEntryOptions
	    {
	        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
	    });
	    return true;
	}
}
}
