using AutoMapper;

namespace EducationHub.Conteudo.Application.AutoMapper
{
    public class AulaMappingProfile : Profile
    {
        public AulaMappingProfile()
        {
            CreateMap<Domain.Entidades.Aula, ViewModels.AulaViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.CursoId, opt => opt.MapFrom(s => s.CursoId));

            CreateMap<ViewModels.AulaViewModel, Domain.Entidades.Aula>()
                .ForCtorParam("titulo", opt => opt.MapFrom(src => src.Titulo))
                .ForCtorParam("conteudoAula", opt => opt.MapFrom(src => src.ConteudoAula))
                .ForCtorParam("materialDeApoio", opt => opt.MapFrom(src => src.MaterialDeApoio))
                .ForCtorParam("duracao", opt => opt.MapFrom(src => src.Duracao))
                .ForCtorParam("cursoId", opt => opt.MapFrom(src => src.CursoId))
                .ForMember(d => d.Curso, opt => opt.Ignore())
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}