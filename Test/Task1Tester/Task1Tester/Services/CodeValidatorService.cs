using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.Text.RegularExpressions;

namespace Task1Tester.Services;

public static class CodeValidatorService
{
    public static (bool Valid, List<Violation> Violations) Validate(string codePath, string expectedLoopType)
    {
        var violations = new List<Violation>();
        try
        {
            if (!File.Exists(codePath))
            {
                violations.Add(new Violation("System Error", $"Code file not found: {codePath}"));
                return (false, violations);
            }

            var extension = (Path.GetExtension(codePath) ?? string.Empty).ToLowerInvariant();
            var code = File.ReadAllText(codePath);

            ValidateHeaders(code, extension, violations);

            if (extension == ".cs")
                ValidateCSharp(code, expectedLoopType, violations);
            else if (extension == ".vb")
                ValidateVB(code, expectedLoopType, violations);
            else
                violations.Add(new Violation("System Error", "Unsupported file extension."));
        }
        catch (Exception ex)
        {
            violations.Add(new Violation("Code Error", $"Analysis failed: {ex.Message}"));
        }

        return (violations.Count == 0, violations);
    }

    private static void ValidateHeaders(string code, string extension, List<Violation> violations)
    {
        string commentSym = extension == ".vb" ? "'" : "//";
        var missingHeaders = new List<string>();
        for (int i = 1; i <= 5; i++)
        {
            string xx = i.ToString("D2");
            string escapedSym = Regex.Escape(commentSym);
            string pattern = $@"\s*{escapedSym}\s*\*{{30}}\r?\n\s*{escapedSym}\s*\* 11900-9403{xx} Program Start \*\r?\n\s*{escapedSym}\s*\*{{30}}";
            if (!Regex.IsMatch(code, pattern)) missingHeaders.Add(xx);
        }
        if (missingHeaders.Count > 0)
            violations.Add(new Violation("Code Violation", $"Rule 5.1: Missing/incorrect headers for: {string.Join(", ", missingHeaders)}"));
    }

    private static void ValidateCSharp(string code, string expectedLoopType, List<Violation> violations)
    {
        var tree = Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(code);
        var root = tree.GetRoot() as Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax;
        if (root == null) return;

        var gotoNodes = root.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.GotoStatementSyntax>();
        foreach (var g in gotoNodes)
            violations.Add(new Violation("Code Violation", $"Rule 6.3: 'goto' found at line {g.GetLocation().GetLineSpan().StartLinePosition.Line + 1}"));

        var invocations = root.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax>();
        string[] forbidden = { "Sort", "Reverse", "Max", "Min", "Sqrt", "Pow" };
        foreach (var inv in invocations)
        {
            var methodName = inv.Expression.ToString();
            if (forbidden.Any(f => methodName.Contains(f)))
                violations.Add(new Violation("Code Violation", $"Rule 6.4: Forbidden method '{methodName}' at line {inv.GetLocation().GetLineSpan().StartLinePosition.Line + 1}"));
        }

        var usings = root.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax>();
        if (usings.Any(u => u.Name?.ToString().Contains("System.Linq") == true))
            violations.Add(new Violation("Code Warning", "Rule 6.4: 'System.Linq' is discouraged; algorithms must be manual."));

        bool hasFor = root.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.ForStatementSyntax>().Any() || 
                      root.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.ForEachStatementSyntax>().Any();
        bool hasWhile = root.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.WhileStatementSyntax>().Any();
        bool hasDo = root.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.DoStatementSyntax>().Any();

        ValidateLoopType(expectedLoopType, hasFor, hasWhile, hasDo, violations);
    }

    private static void ValidateVB(string code, string expectedLoopType, List<Violation> violations)
    {
        var tree = Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxTree.ParseText(code);
        var root = tree.GetRoot() as Microsoft.CodeAnalysis.VisualBasic.Syntax.CompilationUnitSyntax;
        if (root == null) return;

        var gotoNodes = root.DescendantNodes().OfType<Microsoft.CodeAnalysis.VisualBasic.Syntax.GoToStatementSyntax>();
        foreach (var g in gotoNodes)
            violations.Add(new Violation("Code Violation", $"Rule 6.3: 'GoTo' found at line {g.GetLocation().GetLineSpan().StartLinePosition.Line + 1}"));

        bool hasFor = root.DescendantNodes().OfType<Microsoft.CodeAnalysis.VisualBasic.Syntax.ForBlockSyntax>().Any() || 
                      root.DescendantNodes().OfType<Microsoft.CodeAnalysis.VisualBasic.Syntax.ForEachBlockSyntax>().Any();
        
        bool hasWhile = root.DescendantNodes().OfType<Microsoft.CodeAnalysis.VisualBasic.Syntax.WhileBlockSyntax>().Any() ||
                        root.DescendantNodes().OfType<Microsoft.CodeAnalysis.VisualBasic.Syntax.DoLoopBlockSyntax>()
                              .Any(d => d.DoStatement.WhileOrUntilClause?.IsKind(Microsoft.CodeAnalysis.VisualBasic.SyntaxKind.WhileClause) == true);

        bool hasDo = root.DescendantNodes().OfType<Microsoft.CodeAnalysis.VisualBasic.Syntax.DoLoopBlockSyntax>()
                         .Any(d => d.LoopStatement.WhileOrUntilClause != null);

        ValidateLoopType(expectedLoopType, hasFor, hasWhile, hasDo, violations);
    }

    private static void ValidateLoopType(string expected, bool hasFor, bool hasWhile, bool hasDo, List<Violation> violations)
    {
        var used = new List<string>();
        if (hasFor) used.Add("for");
        if (hasWhile) used.Add("while");
        if (hasDo) used.Add("do");

        if (used.Count == 0)
            violations.Add(new Violation("Code Violation", $"Rule 6.2: Missing required '{expected}' loop."));
        else if (used.Count > 1 || used[0] != expected)
            violations.Add(new Violation("Code Violation", $"Rule Violation: Drawn type is '{expected}', but found: {string.Join(", ", used)}."));
    }
}
