using AutoMapper;

namespace EducationHub.Conteudo.Application.AutoMapper
{
    public class AulaMappingProfile : Profile
    {
        public AulaMappingProfile()
        {
            CreateMap<Domain.Entidades.Aula, ViewModels.AulaViewModel>();
            CreateMap<ViewModels.AulaViewModel, Domain.Entidades.Aula>();
        }
    }
}
