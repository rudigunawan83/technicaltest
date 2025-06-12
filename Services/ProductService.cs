using Common.Logging;
using Common.Helpers;
using Grpc.Core;
using technicaltest.Protos;
using technicaltest.UseCases;
using technicaltest.Validators;
using Serilog;

namespace technicaltest.Services
{
	public class ProductService : Protos.ProductGrpcService.ProductGrpcServiceBase
	{
	private readonly IProductUseCase _uc;
	private readonly ILogger<ProductService> _log;

	public ProductService(IProductUseCase uc, ILogger<ProductService> log)
	{
	    _uc = uc ?? throw new ArgumentNullException(nameof(uc));
	    _log = log ?? throw new ArgumentNullException(nameof(log));
	}

	//Tambahkan modeling di proto sesuai kebutuhan
	public async override Task<resProductMessage> Add(ProductModel request, ServerCallContext context)
	{
	    BasicLog basicLog = new BasicLog
	    {
	        TraceId = ContextHelper.GetTraceId(context),
	        TrxId = ContextHelper.GetTrxId(context)
	    };
	    //basicLog.SetMessage($"Message log here");
	    SDLogging.Log(basicLog.ToString());
	    try
	    {
	        //basicLog.SetMessage($"elapsedTime:{basicLog.GetTimeSpan()}");
	        SDLogging.Log(basicLog.ToString());
	        var ret = await _uc.Add(request, context);
	        return ret;
	    }
	    catch (Exception ex)
	    {
	        SDLogging.Log($"Error {ex.Message}", SDLogging.ERROR);
	        context.Status = new Status(StatusCode.Aborted, "Failed insert new Product, error " + ex.Message);
	        return null;
	    }
	}
	}
}
