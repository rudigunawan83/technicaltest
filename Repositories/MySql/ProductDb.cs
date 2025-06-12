using Dapper;
using Google.Rpc;
using technicaltest.Config;
using technicaltest.Models;
using MySql.Data.MySqlClient;

namespace technicaltest.Repositories.MySql
{
	public interface IProductDb
	{
	    Task<Models.Product> Add(Models.Product o);
	    Task<Models.Product> Update(Models.Product o);
	    Task<Models.Product> Delete(Models.Product o);
	    Task<Models.Product> GetByCode(string code);
	    Task<List<Models.Product>> GetAll();
	    Task<List<Models.Product>> GetByPage(Models.Product o, int page, int pageSize, int totalRec);
	}

public class ProductDb : IProductDb
{
	#region SqlCommand
	string table = "product";
	string fields = "Id ,Name ,Description ,Price ,CreatedAt ";
	string fields_insert = "@Id ,@Name ,@Description ,@Price ,@CreatedAt ";
	string fields_update = "Id = @Id, Name = @Name, Description = @Description, Price = @Price, CreatedAt = @CreatedAt";

	#endregion

	private readonly IDbConnectionFactory _conFactory;
	private readonly MySqlConnection _conn;

	public ProductDb(IDbConnectionFactory connectionFactory)
	{
	    _conFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
	    _conn = (MySqlConnection)_conFactory.CreateConnectionAsync().Result;
	}

	public async Task<Models.Product> Add(Models.Product o)
	{
	    var dt = DateTime.UtcNow;
	    string sqlQuery = $"insert into {table} ({fields}) values ({fields_insert})";
	    try
	    {
	        _ = await _conn.ExecuteAsync(sqlQuery, new
	        {
				Id = o.Id,
				Name = o.Name,
				Description = o.Description,
				Price = o.Price,
				CreatedAt = o.CreatedAt
	        });
	        return o;
	    }
	    catch
	    {
	        throw;
	    }
	}
	public async Task<Models.Product> Update(Models.Product o)
	{
	    var dt = DateTime.UtcNow;
	    string sqlQuery = $"update {table} set {fields_update}";
	    try
	    {
	        _ = await _conn.ExecuteAsync(sqlQuery, new
	        {
				
	        });
	        return o;
	    }
	    catch
	    {
	        throw;
	    }
	}
	public async Task<Models.Product> Delete(Models.Product o)
	{
	    var dt = DateTime.UtcNow;
	    string sqlQuery = $"delete from {table} where ";
	    try
	    {
	        _ = await _conn.ExecuteAsync(sqlQuery, new
	        {
				
	        });
	        return o;
	    }
	    catch
	    {
	        throw;
	    }
	}

	public async Task<Models.Product> GetByCode(string code)
	{
	    throw new NotImplementedException();
	}

	public async Task<List<Models.Product>> GetAll()
	{
	    throw new NotImplementedException();
	}

	public async Task<List<Models.Product>> GetByPage(Models.Product o, int page, int pageSize, int totalRec)
	{
	    throw new NotImplementedException();
	}
}
}
