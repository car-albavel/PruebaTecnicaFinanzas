using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SolucionEmpresas.Models;

namespace SolucionEmpresas.Services
{
    public class TraerIndicadores
    {
        public List<Dictionary<string, object>> ObtenerIndicadoresDinamico()
        {
            dbFinanzasEmpresaEntities db = new dbFinanzasEmpresaEntities();

            var filas = new List<Dictionary<string, object>>();

            var conn = db.Database.Connection;

            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "ObtenerIndicadoresActivosConNombre";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var fila = new Dictionary<string, object>();

                        // Recorre TODAS las columnas devueltas por el SP
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string nombreColumna = reader.GetName(i);   // ej: "EmpresaId"
                            object valor = reader.GetValue(i);         // valor de esa columna

                            fila[nombreColumna] = valor;
                        }

                        filas.Add(fila);
                    }
                }
            }

            return filas;
        }
    }


    
}