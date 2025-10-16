using AutoMapper;

namespace EducationHub.Conteudo.Application.AutoMapper
{
    public class CursoMappingProfile : Profile
    {
        public CursoMappingProfile() 
        { 
            CreateMap<Domain.Entidades.Curso, ViewModels.CursoViewModel>()
                .ForMember(dest => dest.ConteudoProgramatico, opt => opt.MapFrom(src => src.ConteudoProgramatico))
                .ForMember(dest => dest.Aulas, opt => opt.MapFrom(src => src.Aulas));
            CreateMap<ViewModels.CursoViewModel, Domain.Entidades.Curso>()
                .ForMember(dest => dest.ConteudoProgramatico, opt => opt.MapFrom(src => src.ConteudoProgramatico))
                .ForMember(dest => dest.Aulas, opt => opt.MapFrom(src => src.Aulas));
        }
    }
}
