using AutoMapper;
using  MozeApi.Entities;
using  MozeApi.DTOs.Response;
using  MozeApi.DTOs.Request;
using  MozeApi.DTOs.UrlScheme;

namespace MozeApi
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            // Entity to Response DTO
            CreateMap<Transaction, TransactionResponse>().ReverseMap();
            CreateMap<Balance, BalanceResponse>().ReverseMap();
            CreateMap<AppUrl, AppUrlResponse>().ReverseMap();

            // Request DTO to Entity
            CreateMap<CreateTransactionRequest, Transaction>();
            CreateMap<UpdateTransactionRequest, Transaction>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CreateAppUrlRequest, AppUrl>();
            CreateMap<UpdateAppUrlRequest, AppUrl>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // URL Scheme DTO to Entity
            CreateMap<ExpenseUrlSchemeDto, Transaction>()
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => TransactionType.Expense));
            CreateMap<IncomeUrlSchemeDto, Transaction>()
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => TransactionType.Income));
            CreateMap<BalanceUrlSchemeDto, Balance>();

        }
    }
}