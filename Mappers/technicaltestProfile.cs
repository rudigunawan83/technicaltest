using AutoMapper;
namespace technicaltest.Mappers
{
    public class technicaltestProfile : Profile
    {
        public technicaltestProfile()
        {
            // Source -> Target
			CreateMap<Models.Product, Protos.ProductModel>().ReverseMap();
        }
    }
}
