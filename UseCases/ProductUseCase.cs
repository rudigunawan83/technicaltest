using FluentValidation;
using Grpc.Core;
using AutoMapper;
using technicaltest.Repositories;
using technicaltest.Protos;

namespace technicaltest.UseCases
{
	public interface IProductUseCase
	{
	    Task<Protos.resProductMessage> Add(Protos.ProductModel o, ServerCallContext context);
	    Task<Protos.ProductModel> GetByCode(string code, ServerCallContext context);
	    Task<Protos.resProductAll> GetAll(Protos.ProductEmpty o, ServerCallContext context);
	}
	public class ProductUseCase : IProductUseCase
	{
	private readonly IProductRepository _repo;
	    private readonly IValidator<Models.Product> _validator;
	    private readonly IMapper _mapper;

	    public ProductUseCase(IProductRepository repo, IValidator<Models.Product> validator, IMapper mapper)
	    {
	        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
	        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	        _validator = validator??throw new ArgumentNullException(nameof(validator));
	    }
	    public async Task<Protos.resProductMessage> Add(Protos.ProductModel o, ServerCallContext context)
	    {
	        try
	        {
	            var oNew = _mapper.Map<Models.Product>(o);

	            var res = await _validator.ValidateAsync(oNew);
	            if (res.IsValid)
	            {
	                _ = await _repo.db().Add(oNew);
	                //_ = await _repo.cache().SetCache(oNew);
	            }
	            else
	            {
	                throw new Exception("Data Error validation");
	            }

	            return new Protos.resProductMessage { Message = "OK" };
	        }
	        catch
	        {
	            throw;
	        }
	    }

	    public Task<resProductAll> GetAll(ProductEmpty o, ServerCallContext context)
	    {
	        throw new NotImplementedException();
	    }

	    public Task<ProductModel> GetByCode(string code, ServerCallContext context)
	    {
	        throw new NotImplementedException();
	    }
	}
}
