using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace EcommerceIS
{
    public static class ExcelExportService
    {
        public static void ExportReport(DataTable data, string reportTitle, string preparedBy, string chartType = "Bar")
        {
            if (data == null || data.Rows.Count == 0)
            {
                MessageBox.Show("No data to export. Please generate a report first.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Files|*.xlsx";
                sfd.FileName = $"{reportTitle.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var wb = new XLWorkbook())
                    {
                        // ── SHEET 1: Report Data ──────────────────
                        var ws = wb.Worksheets.Add("Report");
                        BuildSheet1(ws, data, reportTitle, preparedBy);

                        // ── SHEET 2: Chart Data ───────────────────
                        var ws2 = wb.Worksheets.Add("Chart");
                        BuildSheet2(ws2, data, reportTitle, chartType);

                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show($"Report exported successfully!\n\n📁 {sfd.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Auto-open the file
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = sfd.FileName, UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Export failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static void BuildSheet1(IXLWorksheet ws, DataTable data, string reportTitle, string preparedBy)
        {
            int colCount = data.Columns.Count;

            // ── Logo (embedded image) ─────────────────────
            var logoBytes = GenerateLogoPng();
            using (var ms = new MemoryStream(logoBytes))
            {
                var pic = ws.AddPicture(ms);
                pic.MoveTo(ws.Cell("A1"));
                pic.WithSize(160, 50);
            }

            // ── Company Header ────────────────────────────
            ws.Row(1).Height = 28;
            ws.Row(2).Height = 20;
            ws.Row(3).Height = 18;
            ws.Row(4).Height = 14;

            ws.Cell("D1").Value = "EcommerceDB System";
            ws.Cell("D1").Style.Font.Bold = true;
            ws.Cell("D1").Style.Font.FontSize = 16;
            ws.Cell("D1").Style.Font.FontColor = XLColor.FromArgb(22, 44, 90);
            ws.Range(ws.Cell("D1"), ws.Cell(1, colCount)).Merge();

            ws.Cell("D2").Value = "Bicol University — College of Science — BS Information Technology";
            ws.Cell("D2").Style.Font.FontSize = 9;
            ws.Cell("D2").Style.Font.FontColor = XLColor.Gray;
            ws.Range(ws.Cell("D2"), ws.Cell(2, colCount)).Merge();

            ws.Cell("A3").Value = reportTitle;
            ws.Cell("A3").Style.Font.Bold = true;
            ws.Cell("A3").Style.Font.FontSize = 12;
            ws.Cell("A3").Style.Font.FontColor = XLColor.FromArgb(30, 77, 140);
            ws.Range(ws.Cell("A3"), ws.Cell(3, colCount)).Merge();

            ws.Cell("A4").Value = $"Generated: {DateTime.Now:MMMM dd, yyyy hh:mm tt}";
            ws.Cell("A4").Style.Font.FontSize = 8;
            ws.Cell("A4").Style.Font.FontColor = XLColor.Gray;
            ws.Range(ws.Cell("A4"), ws.Cell(4, colCount)).Merge();

            // ── Column Headers (Row 6) ────────────────────
            int headerRow = 6;
            for (int c = 0; c < colCount; c++)
            {
                var cell = ws.Cell(headerRow, c + 1);
                cell.Value = data.Columns[c].ColumnName;
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Font.FontSize = 10;
                cell.Style.Fill.BackgroundColor = XLColor.FromArgb(22, 44, 90);
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            // ── Data Rows ─────────────────────────────────
            int dataStart = headerRow + 1;
            for (int r = 0; r < data.Rows.Count; r++)
            {
                for (int c = 0; c < colCount; c++)
                {
                    var cell = ws.Cell(dataStart + r, c + 1);
                    cell.Value = data.Rows[r][c]?.ToString() ?? "";
                    cell.Style.Font.FontSize = 9;
                    cell.Style.Border.BottomBorder = XLBorderStyleValues.Hair;
                    cell.Style.Border.BottomBorderColor = XLColor.FromArgb(220, 225, 235);

                    if (r % 2 == 1)
                        cell.Style.Fill.BackgroundColor = XLColor.FromArgb(248, 250, 255);
                }
            }

            // ── Totals / Summary Row ──────────────────────
            int totalRow = dataStart + data.Rows.Count + 1;
            ws.Cell(totalRow, 1).Value = $"Total Records: {data.Rows.Count}";
            ws.Cell(totalRow, 1).Style.Font.Bold = true;
            ws.Cell(totalRow, 1).Style.Font.FontSize = 9;
            ws.Range(ws.Cell(totalRow, 1), ws.Cell(totalRow, 3)).Merge();

            // ── Signature Block ───────────────────────────
            int sigRow = totalRow + 3;
            ws.Cell(sigRow, 1).Value = "Prepared by:";
            ws.Cell(sigRow, 1).Style.Font.Bold = true;
            ws.Cell(sigRow, 1).Style.Font.FontSize = 9;

            ws.Cell(sigRow + 2, 1).Value = preparedBy;
            ws.Cell(sigRow + 2, 1).Style.Font.Bold = true;
            ws.Cell(sigRow + 2, 1).Style.Font.FontSize = 10;
            ws.Cell(sigRow + 2, 1).Style.Font.FontColor = XLColor.FromArgb(30, 77, 140);
            ws.Cell(sigRow + 3, 1).Value = "____________________________";
            ws.Cell(sigRow + 4, 1).Value = "Signature over Printed Name";
            ws.Cell(sigRow + 4, 1).Style.Font.FontSize = 8;
            ws.Cell(sigRow + 4, 1).Style.Font.FontColor = XLColor.Gray;

            int sigCol = Math.Max(4, colCount - 2);
            ws.Cell(sigRow, sigCol).Value = "Approved by:";
            ws.Cell(sigRow, sigCol).Style.Font.Bold = true;
            ws.Cell(sigRow, sigCol).Style.Font.FontSize = 9;

            ws.Cell(sigRow + 2, sigCol).Value = "________________________";
            ws.Cell(sigRow + 3, sigCol).Value = "____________________________";
            ws.Cell(sigRow + 4, sigCol).Value = "Signature over Printed Name";
            ws.Cell(sigRow + 4, sigCol).Style.Font.FontSize = 8;
            ws.Cell(sigRow + 4, sigCol).Style.Font.FontColor = XLColor.Gray;

            // ── Date Signed Row ───────────────────────────
            ws.Cell(sigRow + 5, 1).Value = $"Date: _______________";
            ws.Cell(sigRow + 5, 1).Style.Font.FontSize = 8;
            ws.Cell(sigRow + 5, sigCol).Value = $"Date: _______________";
            ws.Cell(sigRow + 5, sigCol).Style.Font.FontSize = 8;

            // Auto-fit columns
            ws.Columns().AdjustToContents(6, dataStart + data.Rows.Count);
            for (int c = 1; c <= colCount; c++)
            {
                if (ws.Column(c).Width < 12) ws.Column(c).Width = 12;
                if (ws.Column(c).Width > 35) ws.Column(c).Width = 35;
            }

            // Print setup
            ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            ws.PageSetup.FitToPages(1, 0);
        }

        private static void BuildSheet2(IXLWorksheet ws, DataTable data, string reportTitle, string chartType)
        {
            // ── Header ────────────────────────────────────
            ws.Cell("A1").Value = $"{reportTitle} — Chart Data";
            ws.Cell("A1").Style.Font.Bold = true;
            ws.Cell("A1").Style.Font.FontSize = 14;
            ws.Cell("A1").Style.Font.FontColor = XLColor.FromArgb(22, 44, 90);
            ws.Range("A1:E1").Merge();

            ws.Cell("A2").Value = $"Generated: {DateTime.Now:MMMM dd, yyyy}";
            ws.Cell("A2").Style.Font.FontSize = 9;
            ws.Cell("A2").Style.Font.FontColor = XLColor.Gray;

            // ── Aggregate data for chart ──────────────────
            var chartData = AggregateChartData(data, reportTitle);

            // Chart summary table
            int startRow = 4;
            ws.Cell(startRow, 1).Value = "Category";
            ws.Cell(startRow, 2).Value = "Value";
            ws.Cell(startRow, 1).Style.Font.Bold = true;
            ws.Cell(startRow, 2).Style.Font.Bold = true;
            ws.Cell(startRow, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(22, 44, 90);
            ws.Cell(startRow, 2).Style.Fill.BackgroundColor = XLColor.FromArgb(22, 44, 90);
            ws.Cell(startRow, 1).Style.Font.FontColor = XLColor.White;
            ws.Cell(startRow, 2).Style.Font.FontColor = XLColor.White;

            string[] categories = chartData.Item1;
            double[] values = chartData.Item2;

            // Color palette for the bars
            var colors = new[] {
                XLColor.FromArgb(54, 162, 235), XLColor.FromArgb(255, 99, 132),
                XLColor.FromArgb(75, 192, 192), XLColor.FromArgb(255, 159, 64),
                XLColor.FromArgb(153, 102, 255), XLColor.FromArgb(255, 205, 86),
                XLColor.FromArgb(201, 203, 207), XLColor.FromArgb(255, 99, 71),
                XLColor.FromArgb(46, 204, 113), XLColor.FromArgb(52, 152, 219),
                XLColor.FromArgb(155, 89, 182), XLColor.FromArgb(241, 196, 15)
            };

            for (int i = 0; i < categories.Length; i++)
            {
                int row = startRow + 1 + i;
                ws.Cell(row, 1).Value = categories[i];
                ws.Cell(row, 2).Value = values[i];
                ws.Cell(row, 2).Style.NumberFormat.Format = "#,##0.00";
                ws.Cell(row, 1).Style.Font.FontSize = 10;
                ws.Cell(row, 2).Style.Font.FontSize = 10;
                ws.Cell(row, 2).Style.Font.Bold = true;

                // Color coded bar visualization in column C
                ws.Cell(row, 3).Style.Fill.BackgroundColor = colors[i % colors.Length];
                double maxVal = values.Max();
                int barWidth = maxVal > 0 ? (int)Math.Max(1, (values[i] / maxVal) * 20) : 1;
                ws.Cell(row, 3).Value = new string('█', barWidth);
                ws.Cell(row, 3).Style.Font.FontColor = colors[i % colors.Length];
                ws.Cell(row, 3).Style.Font.FontSize = 10;

                if (i % 2 == 1)
                {
                    ws.Cell(row, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(248, 250, 255);
                    ws.Cell(row, 2).Style.Fill.BackgroundColor = XLColor.FromArgb(248, 250, 255);
                }
            }

            // ── Visual Chart using cell formatting ────────
            int chartStartRow = startRow + categories.Length + 3;
            ws.Cell(chartStartRow, 1).Value = $"📊 {reportTitle} — Visual Chart";
            ws.Cell(chartStartRow, 1).Style.Font.Bold = true;
            ws.Cell(chartStartRow, 1).Style.Font.FontSize = 12;
            ws.Cell(chartStartRow, 1).Style.Font.FontColor = XLColor.FromArgb(30, 77, 140);
            ws.Range(ws.Cell(chartStartRow, 1), ws.Cell(chartStartRow, 6)).Merge();

            chartStartRow += 1;
            double max = values.Length > 0 ? values.Max() : 1;

            // Draw horizontal bar chart using cells
            for (int i = 0; i < categories.Length; i++)
            {
                int row = chartStartRow + i;
                ws.Cell(row, 1).Value = categories[i];
                ws.Cell(row, 1).Style.Font.FontSize = 9;
                ws.Cell(row, 1).Style.Font.Bold = true;
                ws.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                int barLen = max > 0 ? (int)Math.Ceiling((values[i] / max) * 30) : 0;
                string bar = new string('█', barLen);
                ws.Cell(row, 2).Value = bar;
                ws.Cell(row, 2).Style.Font.FontColor = colors[i % colors.Length];
                ws.Cell(row, 2).Style.Font.FontSize = 10;
                ws.Range(ws.Cell(row, 2), ws.Cell(row, 5)).Merge();

                ws.Cell(row, 6).Value = values[i];
                ws.Cell(row, 6).Style.NumberFormat.Format = "#,##0.00";
                ws.Cell(row, 6).Style.Font.FontSize = 9;
                ws.Cell(row, 6).Style.Font.Bold = true;
            }

            // Auto-fit
            ws.Column(1).Width = 22;
            ws.Column(2).Width = 18;
            ws.Column(3).Width = 25;

            // Instruction note
            int noteRow = chartStartRow + categories.Length + 2;
            ws.Cell(noteRow, 1).Value = "💡 TIP: Select the data table above (A4:B" + (startRow + categories.Length) + "), then Insert → Chart to create an interactive chart.";
            ws.Cell(noteRow, 1).Style.Font.FontSize = 9;
            ws.Cell(noteRow, 1).Style.Font.Italic = true;
            ws.Cell(noteRow, 1).Style.Font.FontColor = XLColor.FromArgb(100, 110, 140);
            ws.Range(ws.Cell(noteRow, 1), ws.Cell(noteRow, 6)).Merge();
        }

        private static Tuple<string[], double[]> AggregateChartData(DataTable data, string reportTitle)
        {
            var categories = new System.Collections.Generic.List<string>();
            var values = new System.Collections.Generic.List<double>();

            string lower = reportTitle.ToLower();

            if (lower.Contains("sales") || lower.Contains("order detail") || lower.Contains("invoice"))
            {
                // Group by Status
                var groups = data.AsEnumerable()
                    .Where(r => r.Table.Columns.Contains("Status"))
                    .GroupBy(r => r["Status"]?.ToString() ?? "Unknown")
                    .ToList();

                if (groups.Count > 0)
                {
                    foreach (var g in groups)
                    {
                        categories.Add(g.Key);
                        values.Add(g.Count());
                    }
                }
                else
                {
                    categories.Add("Records");
                    values.Add(data.Rows.Count);
                }
            }
            else if (lower.Contains("customer"))
            {
                // Top customers
                int take = Math.Min(data.Rows.Count, 8);
                for (int i = 0; i < take; i++)
                {
                    string name = data.Columns.Contains("Name") ? data.Rows[i]["Name"]?.ToString() :
                                  data.Columns.Contains("Customer") ? data.Rows[i]["Customer"]?.ToString() : $"Row {i + 1}";
                    categories.Add(name ?? $"Row {i + 1}");

                    string valCol = data.Columns.Contains("TotalOrders") ? "TotalOrders" :
                                    data.Columns.Contains("Total") ? "Total" : null;
                    if (valCol != null)
                    {
                        string raw = data.Rows[i][valCol]?.ToString() ?? "0";
                        raw = raw.Replace("₱", "").Replace(",", "").Trim();
                        double.TryParse(raw, out double v);
                        values.Add(v);
                    }
                    else values.Add(1);
                }
            }
            else if (lower.Contains("inventor") || lower.Contains("product") || lower.Contains("stock"))
            {
                // Group by Category
                string catCol = data.Columns.Contains("Category") ? "Category" :
                                data.Columns.Contains("CategoryName") ? "CategoryName" : null;
                string stockCol = data.Columns.Contains("Stock") ? "Stock" :
                                  data.Columns.Contains("StockQuantity") ? "StockQuantity" : null;

                if (catCol != null && stockCol != null)
                {
                    var groups = data.AsEnumerable()
                        .GroupBy(r => r[catCol]?.ToString() ?? "Other")
                        .ToList();
                    foreach (var g in groups)
                    {
                        categories.Add(g.Key);
                        double total = g.Sum(r =>
                        {
                            double.TryParse(r[stockCol]?.ToString() ?? "0", out double v);
                            return v;
                        });
                        values.Add(total);
                    }
                }
                else
                {
                    int take = Math.Min(data.Rows.Count, 10);
                    for (int i = 0; i < take; i++)
                    {
                        categories.Add(data.Rows[i][0]?.ToString() ?? $"Row {i + 1}");
                        values.Add(i + 1);
                    }
                }
            }
            else
            {
                // Generic: use first column as label, count as value
                int take = Math.Min(data.Rows.Count, 10);
                for (int i = 0; i < take; i++)
                {
                    categories.Add(data.Rows[i][0]?.ToString() ?? $"Row {i + 1}");
                    values.Add(i + 1);
                }
            }

            if (categories.Count == 0)
            {
                categories.Add("No Data");
                values.Add(0);
            }

            return Tuple.Create(categories.ToArray(), values.ToArray());
        }

        private static byte[] GenerateLogoPng()
        {
            var bmp = new Bitmap(160, 50);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.White);

                // Icon background circle
                using (var br = new SolidBrush(Color.FromArgb(22, 44, 90)))
                    g.FillEllipse(br, 4, 5, 40, 40);
                using (var f = new Font("Segoe UI Emoji", 16f))
                using (var br = new SolidBrush(Color.White))
                    g.DrawString("🛒", f, br, 8, 10);

                // Company name
                using (var f = new Font("Segoe UI", 13f, FontStyle.Bold))
                using (var br = new SolidBrush(Color.FromArgb(22, 44, 90)))
                    g.DrawString("EcommerceDB", f, br, 48, 6);

                // Subtitle
                using (var f = new Font("Segoe UI", 7f))
                using (var br = new SolidBrush(Color.FromArgb(100, 120, 160)))
                    g.DrawString("Information System", f, br, 50, 30);
            }

            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
