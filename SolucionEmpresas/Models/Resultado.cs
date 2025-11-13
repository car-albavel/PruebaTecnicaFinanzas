using System;
using System.Collections.Generic;

namespace SolucionEmpresas.Models
{
    public class ConsultaIndicadoresFinancieros
    {
        public int EmpresaId { get; set; }
        public string NIT { get; set; }
        public string NombreEmpresa { get; set; }
        public int CuentaId { get; set; }
        public string NumeroCuenta { get; set; }
        public string NombreCuenta { get; set; }
        public DateTime Periodo { get; set; }

        // Propiedades dinámicas adicionales (se llenarán según el SP)
        public Dictionary<string, object> ColumnasAdicionales { get; set; }
    }

    public class FiltrosConsultaIndicadores
    {
        public string NIT { get; set; }
        public int? Anio { get; set; }
        public List<string> ColumnasParaFiltrar { get; set; }
        public Dictionary<string, string> ValoresFiltros { get; set; }
    }

    public class ResultadoConsultaIndicadores
    {
        public List<ConsultaIndicadoresFinancieros> Datos { get; set; }
        public List<string> ColumnasAdicionales { get; set; }
        public FiltrosConsultaIndicadores Filtros { get; set; }
        public int TotalRegistros { get; set; }
    }
}