// filepath: Services/PdfValidatorService.cs
namespace Task1Tester.Services;

public static class PdfValidatorService
{
    /// <summary>
    /// Validates the PDF header for required candidate information.
    /// (119003B14 Page 10, Rule 5)
    /// </summary>
    public static (bool Valid, List<Violation> Violations) ValidateHeader(string pdfPath, HeaderInfo expected)
    {
        var violations = new List<Violation>();
        try
        {
            if (!File.Exists(pdfPath))
            {
                violations.Add(new Violation("System Error", $"User PDF not found: {pdfPath}"));
                return (false, violations);
            }

            using var reader = new PdfReader(pdfPath);
            using var doc = new PdfDocument(reader);
            var firstPage = doc.GetPage(1);
            var text = PdfTextExtractor.GetTextFromPage(firstPage, new SimpleTextExtractionStrategy());

            // Normalize spaces for comparison
            string normalizedText = Regex.Replace(text, @"\s+", "");

            // Page 10 specified header fields: 姓名, 術科測試編號, 座號, 日期
            CheckRequirement(normalizedText, $"姓名：{expected.Name}", "Name", violations);
            CheckRequirement(normalizedText, $"術科測試編號：{expected.TestNo}", "TestNo", violations);
            CheckRequirement(normalizedText, $"座號：{expected.SeatNo}", "SeatNo", violations);

            // Minguo Date: yyy/mm/dd
            var now = DateTime.Now;
            var expectedDateStr = $"日期：{now.Year - 1911}/{now:MM/dd}";
            CheckRequirement(normalizedText, expectedDateStr, "Date", violations);
        }
        catch (Exception ex)
        {
            violations.Add(new Violation("PDF Error", $"Failed to read PDF: {ex.Message}"));
        }

        return (violations.Count == 0, violations);
    }

    /// <summary>
    /// Validates PDF content against ans.pdf for Questions 1 to 5.
    /// (119003B14 Rule: Questions 1-5 must match reference output)
    /// </summary>
    public static (bool Valid, List<Violation> Violations) ValidateContent(string pdfPath, string ansPath)
    {
        var violations = new List<Violation>();
        try
        {
            if (!File.Exists(ansPath))
            {
                violations.Add(new Violation("System Error", $"Reference PDF (ans.pdf) not found at: {ansPath}"));
                return (false, violations);
            }

            using var userReader = new PdfReader(pdfPath);
            using var userDoc = new PdfDocument(userReader);
            using var ansReader = new PdfReader(ansPath);
            using var ansDoc = new PdfDocument(ansReader);

            var userText = GetAllText(userDoc);
            var ansText = GetAllText(ansDoc);

            // Questions 1 to 5
            string[] headers = [
                "第一題結果", 
                "第二題結果", 
                "第三題結果", 
                "第四題結果", 
                "第五題結果"
            ];

            foreach (var header in headers)
            {
                var userSection = ExtractSection(userText, header);
                var ansSection = ExtractSection(ansText, header);

                if (string.IsNullOrWhiteSpace(userSection))
                {
                    violations.Add(new Violation("PDF Content", $"Rule Violation: Missing section output for '{header}'"));
                }
                else if (NormalizeForCompare(userSection) != NormalizeForCompare(ansSection))
                {
                    violations.Add(new Violation("PDF Content", $"Rule Violation: Output mismatch for '{header}'"));
                }
            }
        }
        catch (Exception ex)
        {
            violations.Add(new Violation("PDF Error", $"Failed to validate PDF content: {ex.Message}"));
        }

        return (violations.Count == 0, violations);
    }

    private static void CheckRequirement(string text, string pattern, string label, List<Violation> violations)
    {
        string normalizedPattern = Regex.Replace(pattern, @"\s+", "");
        if (!text.Contains(normalizedPattern))
        {
            violations.Add(new Violation("PDF Header", $"Missing or incorrect {label}. Expected pattern: '{pattern}'"));
        }
    }

    private static string GetAllText(PdfDocument doc)
    {
        var sb = new StringBuilder();
        for (int i = 1; i <= doc.GetNumberOfPages(); i++)
        {
            sb.Append(PdfTextExtractor.GetTextFromPage(doc.GetPage(i), new SimpleTextExtractionStrategy()));
            sb.Append('\n');
        }
        return sb.ToString();
    }

    private static string ExtractSection(string fullText, string header)
    {
        var lines = fullText.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);
        var startIndex = Array.FindIndex(lines, l => l.Contains(header));
        if (startIndex == -1) return string.Empty;

        // Take lines until the next section marker or specific end indicators
        var sectionLines = lines.Skip(startIndex)
                               .TakeWhile((line, index) => index == 0 || !line.Contains("題結果"))
                               .Select(l => l.Trim());
        
        return string.Join("\n", sectionLines).Trim();
    }

    private static string NormalizeForCompare(string content)
    {
        // Remove spaces, tabs, and specific characters that might differ in rendering but not in data
        return Regex.Replace(content, @"\s+", "");
    }
}
