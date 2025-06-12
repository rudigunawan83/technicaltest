using technicaltest.Repositories.Cache;
using technicaltest.Repositories.MySql;
using technicaltest.Repositories;

namespace technicaltest.Repositories
{
	public interface IProductRepository {
		IProductDb db();
		IProductCache cache();
	}

public class ProductRepository: IProductRepository 
{
	private readonly IProductDb _Db;
	private readonly IProductCache _Cache;

	public ProductRepository(IProductDb Db, IProductCache Cache)
	{
	    _Db = Db ?? throw new ArgumentNullException(nameof(Db));
	    _Cache = Cache ?? throw new ArgumentNullException(nameof(Cache));
	}

	public IProductDb db()
	{
	    return _Db;
	}

	public IProductCache cache()
	{
	    return _Cache;
	}
}
}
