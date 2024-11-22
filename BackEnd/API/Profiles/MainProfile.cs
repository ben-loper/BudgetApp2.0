using AutoMapper;
using BackEnd.DTOs.BudgetDtos;
using BackEnd.DTOs.FamilyDtos;
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

            CreateMap<BudgetDto, Budget>().ReverseMap();
            CreateMap<MonthlyBillDto, MonthlyBill>().ReverseMap();
            CreateMap<TransactionDto, Transaction>().ReverseMap();
            CreateMap<BudgetCategoryDto, BudgetCategory>()
                .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id != null))
                .ReverseMap();

        }
    }
}
