using AutoMapper;
using AutoMapper.Execution;
using Talabat.Api.DTOs;
using Talabat.DAL.Entities;

namespace Talabat.Api.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration=configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["ApiUrl"]}{source.PictureUrl}";
            return null;
        }
    }
}
