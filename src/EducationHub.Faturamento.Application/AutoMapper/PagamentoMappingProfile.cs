using AutoMapper;
using EducationHub.Faturamento.Application.ViewModels;
using EducationHub.Faturamento.Domain.Entidades;

namespace EducationHub.Faturamento.Application.AutoMapper
{
    public class PagamentoMappingProfile : Profile
    {
        public PagamentoMappingProfile() 
        {
            CreateMap<Pagamento, PagamentoViewModel>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

            CreateMap<PagamentoViewModel, Pagamento>()
                .ForMember(d => d.TokenCartao, opt => opt.Ignore())
                .ForMember(d => d.NumeroCartaoMascarado, opt => opt.Ignore());
        }
    }
}
