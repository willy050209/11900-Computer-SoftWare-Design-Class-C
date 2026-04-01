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
            string normalizedText = NormalizeForCompare(text ?? string.Empty);

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
                    violations.Add(new Violation("PDF Content", $"Rule Violation: Missing section output for '{header}'. Could not find this section in your PDF."));
                }
                else if (NormalizeForCompare(userSection) != NormalizeForCompare(ansSection))
                {
                    string userPreview = userSection.Length > 100 ? userSection.Substring(0, 100) + "..." : userSection;
                    string ansPreview = ansSection.Length > 100 ? ansSection.Substring(0, 100) + "..." : ansSection;
                    
                    violations.Add(new Violation("PDF Content", 
                        $"Rule Violation: Output mismatch for '{header}'.\n" +
                        $"Expected (start): {ansPreview}\n" +
                        $"Actual (start):   {userPreview}"));
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
        string normalizedPattern = NormalizeForCompare(pattern);
        if (!text.Contains(normalizedPattern))
        {
            violations.Add(new Violation("PDF Header", $"Missing or incorrect {label}. The PDF content does not contain the required string: '{pattern}'. Please ensure the header format is exactly as required."));
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
        string normalizedHeader = NormalizeForCompare(header);
        var startIndex = Array.FindIndex(lines, l => NormalizeForCompare(l).Contains(normalizedHeader));
        if (startIndex == -1) return string.Empty;

        // Take lines until the next section marker or specific end indicators
        // Use normalized check for "題結果" to be consistent
        string normalizedBoundary = NormalizeForCompare("題結果");
        var sectionLines = lines.Skip(startIndex)
                               .TakeWhile((line, index) => index == 0 || !NormalizeForCompare(line).Contains(normalizedBoundary))
                               .Select(l => l.Trim());
        
        return string.Join("\n", sectionLines).Trim();
    }

    private static string NormalizeForCompare(string content)
    {
        if (string.IsNullOrEmpty(content)) return string.Empty;

        // Convert full-width to half-width
        var chars = content.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] == 0x3000) // Full-width space
            {
                chars[i] = (char)0x0020;
            }
            else if (chars[i] >= 0xFF01 && chars[i] <= 0xFF5E)
            {
                chars[i] = (char)(chars[i] - 0xFEE0);
            }
        }
        string normalized = new(chars);

        // Remove spaces, tabs, and specific characters that might differ in rendering but not in data
        return Regex.Replace(normalized, @"\s+", "");
    }
}
