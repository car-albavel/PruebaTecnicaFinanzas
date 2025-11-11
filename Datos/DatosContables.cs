using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DatosContables
    {
        public string NIT { get; set; }
        public string NumeroCuenta { get; set; }
        public string NombreCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal Debitos { get; set; }
        public decimal Creditos { get; set; }
        public decimal SaldoFinal { get; set; }
        public DateTime Periodo { get; set; }
        
    }
}
