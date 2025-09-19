using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;

namespace KorisnickiInterfejs
{
    /// <summary>
    /// Univerzalna klasa za export funkcionalnost - CSV i HTML stampa
    /// Može se koristiti na svim stranicama za export podataka
    /// </summary>
    public static class ExportUtility
    {
        /// <summary>
        /// Exportuje DataTable u CSV format
        /// </summary>
        /// <param name="data">DataTable sa podacima</param>
        /// <param name="filename">Naziv fajla (bez ekstenzije)</param>
        /// <param name="response">HttpResponse objekat</param>
        public static void ExportToCSV(DataTable data, string filename, HttpResponse response)
        {
            if (data == null || data.Rows.Count == 0)
            {
                throw new InvalidOperationException("Nema podataka za export.");
            }

            string csvContent = GenerateCSVContent(data);
            
            // Postavi response headers za download
            response.Clear();
            response.ContentType = "text/csv";
            response.AddHeader("Content-Disposition", $"attachment; filename={filename}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            response.ContentEncoding = Encoding.UTF8;
            
            // Dodaj BOM za UTF-8 da Excel pravilno prikaže srpske karaktere
            response.BinaryWrite(Encoding.UTF8.GetPreamble());
            response.Write(csvContent);
            response.End();
        }

        /// <summary>
        /// Exportuje DataTable u HTML format optimizovan za stampu
        /// </summary>
        /// <param name="data">DataTable sa podacima</param>
        /// <param name="filename">Naziv fajla (bez ekstenzije)</param>
        /// <param name="title">Naslov stampe</param>
        /// <param name="description">Opis stampe</param>
        /// <param name="activeFilters">Lista aktivnih filtera za prikaz</param>
        /// <param name="response">HttpResponse objekat</param>
        public static void ExportToHTML(DataTable data, string filename, string title, string description, 
            List<string> activeFilters, HttpResponse response)
        {
            if (data == null || data.Rows.Count == 0)
            {
                throw new InvalidOperationException("Nema podataka za stampu.");
            }

            string htmlContent = GenerateHTMLContent(data, title, description, activeFilters);
            
            // Postavi response headers za HTML stampu
            response.Clear();
            response.ContentType = "text/html";
            response.AddHeader("Content-Disposition", $"inline; filename={filename}_{DateTime.Now:yyyyMMdd_HHmmss}.html");
            response.Write(htmlContent);
            response.End();
        }

        /// <summary>
        /// Generiše CSV sadržaj od DataTable-a
        /// </summary>
        private static string GenerateCSVContent(DataTable data)
        {
            var csv = new StringBuilder();
            
            // Dodaj header
            var headers = new List<string>();
            foreach (DataColumn column in data.Columns)
            {
                headers.Add(EscapeCSVField(column.ColumnName));
            }
            csv.AppendLine(string.Join(",", headers));
            
            // Dodaj podatke
            foreach (DataRow row in data.Rows)
            {
                var values = new List<string>();
                foreach (DataColumn column in data.Columns)
                {
                    values.Add(EscapeCSVField(row[column]?.ToString() ?? ""));
                }
                csv.AppendLine(string.Join(",", values));
            }
            
            return csv.ToString();
        }

        /// <summary>
        /// Generiše HTML sadržaj optimizovan za stampu
        /// </summary>
        private static string GenerateHTMLContent(DataTable data, string title, string description, List<string> activeFilters)
        {
            var html = new StringBuilder();
            
            // HTML header sa CSS stilovima za stampu
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<meta charset='utf-8'>");
            html.AppendLine($"<title>{title} - Skupština</title>");
            html.AppendLine("<style>");
            html.AppendLine("body { font-family: Arial, sans-serif; font-size: 12px; margin: 20px; background: white; }");
            html.AppendLine(".header { text-align: center; margin-bottom: 30px; border-bottom: 2px solid #333; padding-bottom: 10px; }");
            html.AppendLine(".header h1 { margin: 0; color: #333; font-size: 24px; }");
            html.AppendLine(".header p { margin: 5px 0; color: #666; }");
            html.AppendLine(".info { margin-bottom: 20px; font-size: 11px; color: #666; }");
            html.AppendLine("table { width: 100%; border-collapse: collapse; margin-top: 20px; }");
            html.AppendLine("th, td { border: 1px solid #333; padding: 8px; text-align: left; }");
            html.AppendLine("th { background-color: #f5f5f5; font-weight: bold; }");
            html.AppendLine("tr:nth-child(even) { background-color: #f9f9f9; }");
            html.AppendLine(".footer { margin-top: 30px; text-align: center; font-size: 10px; color: #666; border-top: 1px solid #333; padding-top: 10px; }");
            html.AppendLine(".print-button { position: fixed; top: 20px; right: 20px; background: #007bff; color: white; border: none; padding: 10px 20px; border-radius: 5px; cursor: pointer; font-size: 14px; }");
            html.AppendLine(".print-button:hover { background: #0056b3; }");
            html.AppendLine("@media print {");
            html.AppendLine("  .print-button { display: none; }");
            html.AppendLine("  body { margin: 0; }");
            html.AppendLine("}");
            html.AppendLine("</style>");
            html.AppendLine("<script>");
            html.AppendLine("function printDocument() { window.print(); }");
            html.AppendLine("</script>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            
            // Print button
            html.AppendLine("<button class='print-button' onclick='printDocument()'>🖨️ Štampaj</button>");
            
            // Header
            html.AppendLine("<div class='header'>");
            html.AppendLine($"<h1> {title.ToUpper()}</h1>");
            html.AppendLine($"<p>{description}</p>");
            html.AppendLine("</div>");
            
            // Informacije o filterima
            html.AppendLine("<div class='info'>");
            html.AppendLine($"<strong>Datum generisanja:</strong> {DateTime.Now:dd.MM.yyyy HH:mm}<br>");
            html.AppendLine($"<strong>Ukupno zapisa:</strong> {data.Rows.Count}<br>");
            
            if (activeFilters != null && activeFilters.Count > 0)
            {
                html.AppendLine($"<strong>Primenjeni filteri:</strong> {string.Join(", ", activeFilters)}<br>");
            }
            html.AppendLine("</div>");
            
            // Tabela sa podacima
            html.AppendLine("<table>");
            html.AppendLine("<thead>");
            html.AppendLine("<tr>");
            
            // Header kolone
            foreach (DataColumn column in data.Columns)
            {
                html.AppendLine($"<th>{EscapeHtml(column.ColumnName)}</th>");
            }
            html.AppendLine("</tr>");
            html.AppendLine("</thead>");
            html.AppendLine("<tbody>");
            
            // Podaci
            foreach (DataRow row in data.Rows)
            {
                html.AppendLine("<tr>");
                foreach (DataColumn column in data.Columns)
                {
                    html.AppendLine($"<td>{EscapeHtml(row[column]?.ToString() ?? "")}</td>");
                }
                html.AppendLine("</tr>");
            }
            
            html.AppendLine("</tbody>");
            html.AppendLine("</table>");
            
            // Footer
            html.AppendLine("<div class='footer'>");
            html.AppendLine($"Stampa generisana: {DateTime.Now:dd.MM.yyyy HH:mm} | Ukupno zapisa: {data.Rows.Count}");
            html.AppendLine("</div>");
            
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            
            return html.ToString();
        }

        /// <summary>
        /// Escape-uje CSV polja koja sadrže zareze ili navodnike
        /// </summary>
        private static string EscapeCSVField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "";
                
            // Ako polje sadrži zarez, navodnik ili novi red, okruži ga navodnicima
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                // Escape postojeće navodnike
                field = field.Replace("\"", "\"\"");
                return $"\"{field}\"";
            }
            
            return field;
        }

        /// <summary>
        /// Escape-uje HTML karaktere
        /// </summary>
        private static string EscapeHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
                
            return text.Replace("&", "&amp;")
                      .Replace("<", "&lt;")
                      .Replace(">", "&gt;")
                      .Replace("\"", "&quot;")
                      .Replace("'", "&#39;");
        }
    }
}
