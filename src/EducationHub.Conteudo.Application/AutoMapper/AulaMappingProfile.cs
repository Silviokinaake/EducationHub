using AutoMapper;

namespace EducationHub.Conteudo.Application.AutoMapper
{
    public class AulaMappingProfile : Profile
    {
        public AulaMappingProfile()
        {
            CreateMap<Domain.Entidades.Aula, ViewModels.AulaViewModel>();
            CreateMap<ViewModels.AulaViewModel, Domain.Entidades.Aula>();
            
            // Mapeamentos para os ViewModels de criação e atualização
            CreateMap<ViewModels.CriarAulaViewModel, ViewModels.AulaViewModel>();
            CreateMap<ViewModels.AtualizarAulaViewModel, ViewModels.AulaViewModel>();
        }
    }
}
