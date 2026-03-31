// filepath: Program.cs

Console.WriteLine("=== Level C Computer Software Design: Task 1 Automation Tester ===");

// 1. Argument Parsing
if (args.Length < 6)
{
    Console.WriteLine("Usage: dotnet run -- <code_path> <user_pdf_path> <ans_pdf_path> <name> <test_no> <seat_no>");
    Console.WriteLine("Example: dotnet run -- \"../Task1/Program.cs\" \"../Task1/output.pdf\" \"../../ans.pdf\" \"王小明\" \"112590001\" \"001\"");
    return;
}

var codePath = args[0];
var userPdfPath = args[1];
var ansPdfPath = args[2];
var name = args[3];
var testNo = args[4];
var seatNo = args[5];

var expectedHeader = new HeaderInfo(name, testNo, seatNo);

// 2. Run Validations
var violations = new List<Violation>();

// Code Validation
Console.WriteLine("\n[1/3] Validating Code Integrity...");
var codeResult = CodeValidatorService.Validate(codePath);
violations.AddRange(codeResult.Violations);

// PDF Header Validation
Console.WriteLine("[2/3] Validating PDF Header...");
var headerResult = PdfValidatorService.ValidateHeader(userPdfPath, expectedHeader);
violations.AddRange(headerResult.Violations);

// PDF Content Validation
Console.WriteLine("[3/3] Validating PDF Content (Question 1-5)...");
var contentResult = PdfValidatorService.ValidateContent(userPdfPath, ansPdfPath);
violations.AddRange(contentResult.Violations);

// 3. Final Report
Console.WriteLine("\n=== Validation Report ===");

if (violations.Count == 0)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("✓ ALL TESTS PASSED: Code and PDF follow all requirements.");
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"✗ FAILED: {violations.Count} violation(s) found.");
    foreach (var v in violations)
    {
        Console.WriteLine($"  [{v.Category}] {v.Message}");
    }
}

Console.ResetColor();
