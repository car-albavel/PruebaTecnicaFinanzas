using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DatosContables
    {

        public int EmpresaId { get; set; }
        public DateTime Periodo { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaCargue { get; set; }
        public bool Activa { get; set; }
        public decimal ActivoCorriente { get; set; }
        public decimal PasivoCorriente { get; set; }
        public decimal ActivoTotal { get; set; }
        public decimal PasivoTotal { get; set; }
        public decimal Patrimonio { get; set; }
        public decimal IngresosOperacionales { get; set; }
        public decimal UtilidadBruta { get; set; }
        public decimal UtilidadOperativa { get; set; }
        public decimal UtilidadNeta { get; set; }

    }
}
