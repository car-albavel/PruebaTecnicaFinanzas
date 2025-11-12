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


                    var dato2 = DateTime.Parse(worksheet.Cell($"C{row}").Value.ToString());

                    try
                    {
                        var dato = new Datos.DatosContables
                        {
                            EmpresaId = Convert.ToInt32(worksheet.Cell($"A{row}").Value.ToString()),
                            Periodo = DateTime.Parse(worksheet.Cell($"B{row}").Value.ToString()),
                            Fecha = DateTime.Parse(worksheet.Cell($"C{row}").Value.ToString()),
                            ActivoCorriente = Convert.ToDecimal(worksheet.Cell($"D{row}").Value.ToString()),
                            PasivoCorriente = Convert.ToDecimal(worksheet.Cell($"E{row}").Value.ToString()),
                            ActivoTotal = Convert.ToDecimal(worksheet.Cell($"F{row}").Value.ToString()),
                            PasivoTotal = Convert.ToDecimal(worksheet.Cell($"G{row}").Value.ToString()),
                            Patrimonio = Convert.ToDecimal(worksheet.Cell($"H{row}").Value.ToString()),
                            IngresosOperacionales = Convert.ToDecimal(worksheet.Cell($"I{row}").Value.ToString()),
                            UtilidadBruta = Convert.ToDecimal(worksheet.Cell($"J{row}").Value.ToString()),
                            UtilidadOperativa = Convert.ToDecimal(worksheet.Cell($"K{row}").Value.ToString()),
                            UtilidadNeta = Convert.ToDecimal(worksheet.Cell($"L{row}").Value.ToString()),
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
