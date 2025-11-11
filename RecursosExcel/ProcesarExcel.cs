using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using ClosedXML.Excel;

namespace RecursosExcel
{
    public class ProcesarExcel
    {
        public List<DatosContables> LeerArchivoExcel(string rutaArchivo)
        {
            var datos = new List<DatosContables>();

            using(var workbook = new XLWorkbook(rutaArchivo))
            {
                var worksheet = workbook.Worksheet("Hoja1");

                // Leer desde fila 2 (fila 1 es encabezado)
                for (int row = 2; row <= worksheet.LastRowUsed().RowNumber(); row++)
                {
                    var cellA = worksheet.Cell($"A{row}").Value;

                    if (string.IsNullOrWhiteSpace(cellA.ToString()))
                        continue;


                    try
                    {
                        var dato = new Datos.DatosContables
                        {
                            NIT = worksheet.Cell($"A{row}").Value.ToString() ?? "",
                            NumeroCuenta = worksheet.Cell($"B{row}").Value.ToString() ?? "",
                            NombreCuenta = worksheet.Cell($"C{row}").Value.ToString() ?? "",
                            SaldoInicial = Convert.ToDecimal(worksheet.Cell($"D{row}").Value.ToString()),
                            Debitos = Convert.ToDecimal(worksheet.Cell($"E{row}").Value.ToString()),
                            Creditos = Convert.ToDecimal(worksheet.Cell($"F{row}").Value.ToString()),
                            SaldoFinal = Convert.ToDecimal(worksheet.Cell($"G{row}").Value.ToString()),
                            Periodo = DateTime.Parse(worksheet.Cell($"H{row}").Value.ToString())
                        };

                        datos.Add(dato);
                    }
                    catch (Exception ex)
                    {
                        // Registrar error pero continuar
                        System.Diagnostics.Debug.WriteLine($"Error en fila {row}: {ex.Message}");
                    }
                }
            }

            return datos;
        }
    }
}
