using AutoMapper;
using Ots.Api.Domain;
using Ots.Schema;

namespace Ots.Api.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<CustomerRequest, Customer>();
        CreateMap<Customer, CustomerResponse>();
    }
}