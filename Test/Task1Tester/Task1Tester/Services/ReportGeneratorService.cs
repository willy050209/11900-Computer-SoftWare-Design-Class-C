using System.Text;
using Task1Tester.Models;

namespace Task1Tester.Services;

public static class ReportGeneratorService
{
    public static void GenerateHtmlReport(string outputPath, HeaderInfo header, string loopType, List<Violation> violations)
    {
        bool isPassed = violations.Count == 0;
        var sb = new StringBuilder();

        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang='zh-Hant'>");
        sb.AppendLine("<head>");
        sb.AppendLine("    <meta charset='UTF-8'>");
        sb.AppendLine("    <meta name='viewport' content='width=device-width, initial-scale=1.0'>");
        sb.AppendLine("    <title>第一站自動化測試報告</title>");
        sb.AppendLine("    <style>");
        sb.AppendLine("        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; max-width: 900px; margin: 0 auto; padding: 20px; background-color: #f4f7f9; }");
        sb.AppendLine("        .header { background: #2c3e50; color: #fff; padding: 20px; border-radius: 8px 8px 0 0; text-align: center; }");
        sb.AppendLine("        .summary { background: #fff; padding: 20px; border-radius: 0 0 8px 8px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); margin-bottom: 20px; }");
        sb.AppendLine("        .status { font-size: 24px; font-weight: bold; margin-bottom: 15px; display: flex; align-items: center; justify-content: center; }");
        sb.AppendLine("        .status.pass { color: #27ae60; }");
        sb.AppendLine("        .status.fail { color: #e74c3c; }");
        sb.AppendLine("        .info-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 15px; margin-top: 20px; }");
        sb.AppendLine("        .info-item { background: #f8f9fa; padding: 10px; border-radius: 4px; border-left: 4px solid #3498db; }");
        sb.AppendLine("        .info-label { font-weight: bold; color: #7f8c8d; font-size: 12px; text-transform: uppercase; }");
        sb.AppendLine("        .info-value { font-size: 16px; margin-top: 5px; }");
        sb.AppendLine("        .violations-title { font-size: 20px; margin: 30px 0 15px; border-bottom: 2px solid #ddd; padding-bottom: 5px; }");
        sb.AppendLine("        .violation-card { background: #fff; border-radius: 8px; box-shadow: 0 2px 5px rgba(0,0,0,0.05); margin-bottom: 15px; border-left: 5px solid #e74c3c; overflow: hidden; }");
        sb.AppendLine("        .violation-header { background: #fdf2f2; padding: 10px 15px; font-weight: bold; color: #c0392b; display: flex; justify-content: space-between; }");
        sb.AppendLine("        .violation-body { padding: 15px; white-space: pre-wrap; font-family: 'Consolas', monospace; font-size: 14px; }");
        sb.AppendLine("        .no-violations { text-align: center; padding: 40px; color: #7f8c8d; background: #fff; border-radius: 8px; }");
        sb.AppendLine("        .footer { text-align: center; margin-top: 40px; color: #95a5a6; font-size: 12px; }");
        sb.AppendLine("    </style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");

        sb.AppendLine("    <div class='header'><h1>第一站自動化測試報告</h1></div>");
        sb.AppendLine("    <div class='summary'>");
        
        if (isPassed)
        {
            sb.AppendLine("        <div class='status pass'>✓ 測試通過：所有規則均符合規範</div>");
        }
        else
        {
            sb.AppendLine($"        <div class='status fail'>✗ 測試失敗：發現 {violations.Count} 處違規</div>");
        }

        sb.AppendLine("        <div class='info-grid'>");
        sb.AppendLine($"            <div class='info-item'><div class='info-label'>應檢人姓名</div><div class='info-value'>{header.Name}</div></div>");
        sb.AppendLine($"            <div class='info-item'><div class='info-label'>術科測試編號</div><div class='info-value'>{header.TestNo}</div></div>");
        sb.AppendLine($"            <div class='info-item'><div class='info-label'>座號</div><div class='info-value'>{header.SeatNo}</div></div>");
        sb.AppendLine($"            <div class='info-item'><div class='info-label'>抽籤迴圈類型</div><div class='info-value'>{loopType.ToUpper()}</div></div>");
        sb.AppendLine($"            <div class='info-item'><div class='info-label'>測試時間</div><div class='info-value'>{DateTime.Now:yyyy/MM/dd HH:mm:ss}</div></div>");
        sb.AppendLine("        </div>");
        sb.AppendLine("    </div>");

        if (!isPassed)
        {
            sb.AppendLine("    <h2 class='violations-title'>違規項目明細</h2>");
            foreach (var v in violations)
            {
                sb.AppendLine("    <div class='violation-card'>");
                sb.AppendLine($"        <div class='violation-header'><span>{v.Category}</span></div>");
                sb.AppendLine($"        <div class='violation-body'>{System.Web.HttpUtility.HtmlEncode(v.Message)}</div>");
                sb.AppendLine("    </div>");
            }
        }
        else
        {
            sb.AppendLine("    <div class='no-violations'>");
            sb.AppendLine("        <p style='font-size: 48px; margin: 0;'>✓</p>");
            sb.AppendLine("        <p>優異！您的程式碼與 PDF 檔案完美符合檢定手冊要求。</p>");
            sb.AppendLine("    </div>");
        }

        sb.AppendLine("    <div class='footer'>測試自動化套件</div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);
    }
}
