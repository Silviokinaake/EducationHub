using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationHub.Faturamento.Application.ViewModels
{
    public class PagamentoViewModel
    {
        public Guid Id { get; set; }
        public Guid AlunoId { get; set; }
        public Guid PreMatriculaId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public string Status { get; set; }
        public string TokenCartao { get; set; }
        public string NumeroCartaoMascarado { get; set; }
    }
}
