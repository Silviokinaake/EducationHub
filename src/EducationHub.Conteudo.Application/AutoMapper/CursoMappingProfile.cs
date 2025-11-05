using AutoMapper;
using EducationHub.Conteudo.Application.ViewModels;
using EducationHub.Conteudo.Domain.Entidades;

namespace EducationHub.Conteudo.Application.AutoMapper
{
    public class CursoMappingProfile : Profile
    {
        public CursoMappingProfile()
        {
            CreateMap<Curso, CursoViewModel>()
                .ForMember(dest => dest.ConteudoProgramatico, opt => opt.MapFrom(src => src.ConteudoProgramatico))
                .ForMember(dest => dest.Aulas, opt => opt.MapFrom(src => src.Aulas))
                .ReverseMap();

            CreateMap<Aula, AulaViewModel>().ReverseMap();

            CreateMap<ConteudoProgramatico, ConteudoProgramaticoViewModel>().ReverseMap();
        }
    }
}
