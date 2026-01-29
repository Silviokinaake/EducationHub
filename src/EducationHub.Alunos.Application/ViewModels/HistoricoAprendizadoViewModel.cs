using System;

namespace EducationHub.Alunos.Application.ViewModels
{
    public class HistoricoAprendizadoViewModel
    {
        public Guid CursoId { get; set; }
        public string NomeCurso { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataConclusao { get; set; }
    }
}
