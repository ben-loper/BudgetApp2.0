using AutoMapper;
using BackEnd.DTOs.BudgetDtos;
using BackEnd.DTOs.FamilyDtos;
using BackEnd.DTOs.TransactionDtos.Requests;
using Domain.Models;

namespace BackEnd.Profiles
{
    
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            // Ignore the user mappings. These need to be transformed from UserID to username
            CreateMap<FamilyDto, Family>()
                .ForMember(f => f.MemberUserIds, opt => opt.Ignore())
                .ForMember(f => f.AdminUserIds, opt => opt.Ignore())
                .ReverseMap();

            // Instantiating the object calls the constructor which generates an Id. But then Automapper
            // maps the Id value from the DTO to entity object. 
            // This overwrites the generated Id. This keeps Automapper from overwriting it
            CreateMap<BudgetDto, Budget>()
                .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id != null))
                .ReverseMap();

            CreateMap<MonthlyBillDto, MonthlyBill>()
                .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id != null))
                .ReverseMap();

            CreateMap<TransactionDto, Transaction>()
                .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id != null))
                .ReverseMap();

            CreateMap<BudgetCategoryDto, BudgetCategory>()
                .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id != null))
                .ReverseMap();


            CreateMap<UpdateTransactionRequest, Transaction>();
            CreateMap<AddTransactionRequest, Transaction>();
        }
    }
}
