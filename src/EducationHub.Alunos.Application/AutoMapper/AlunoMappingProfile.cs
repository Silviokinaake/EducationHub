using AutoMapper;
using EducationHub.Alunos.Application.ViewModels;
using EducationHub.Alunos.Domain.Entidades;

namespace EducationHub.Alunos.Application.AutoMapper
{
    public class AlunoMappingProfile : Profile
    {
        public AlunoMappingProfile()
        {
            CreateMap<Aluno, AlunoViewModel>();
            CreateMap<AlunoViewModel, Aluno>()
                .ConstructUsing(vm => new Aluno(vm.Id == Guid.Empty ? Guid.NewGuid() : vm.Id, vm.UsuarioId, vm.Nome, vm.Email, vm.DataNascimento));

            CreateMap<Matricula, MatriculaViewModel>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
            CreateMap<MatriculaViewModel, Matricula>();

            CreateMap<Certificado, CertificadoViewModel>();
            CreateMap<CertificadoViewModel, Certificado>();
        }
    }
}
