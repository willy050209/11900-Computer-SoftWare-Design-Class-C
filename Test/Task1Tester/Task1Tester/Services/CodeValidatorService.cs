// filepath: Services/CodeValidatorService.cs
namespace Task1Tester.Services;

public static class CodeValidatorService
{
    /// <summary>
    /// Validates source code (C# or VB.NET) against 119003B14 Level C requirements.
    /// </summary>
    public static (bool Valid, List<Violation> Violations) Validate(string codePath)
    {
        var violations = new List<Violation>();
        try
        {
            if (!File.Exists(codePath))
            {
                violations.Add(new Violation("System Error", $"Code file not found: {codePath}"));
                return (false, violations);
            }

            var extension = Path.GetExtension(codePath).ToLowerInvariant();
            if (extension != ".cs" && extension != ".vb")
            {
                violations.Add(new Violation("System Error", $"Unsupported code file extension: {extension}. Only .cs and .vb are supported."));
                return (false, violations);
            }

            var code = File.ReadAllText(codePath);

            // 1. Rule: No 'Go To' (119003B14 Page 1, Rule 6.3)
            var gotoMatch = Regex.Match(code, @"\bgoto\b", RegexOptions.IgnoreCase);
            if (gotoMatch.Success)
            {
                int lineNo = code.Take(gotoMatch.Index).Count(c => c == '\n') + 1;
                violations.Add(new Violation("Code Violation", $"Rule 6.3: Use of 'Go To' is strictly prohibited. Found on line {lineNo}."));
            }

            // 2. Rule: Use only built-in/system provided I/O and conversion (Page 1, Rule 6.4)
            // Allowed: File handling, Data conversion, Delimiter handling, Printing.
            // Prohibited: System functions like Math.Sqrt, Array.Sort (must be manual).
            
            // Check for unauthorized namespaces or complex LINQ methods
            string[] suspiciousNamespaces = [
                "System.Linq", 
                "System.Collections.Generic.Enumerable",
                "System.Reflection",
                "System.Runtime"
            ];

            foreach (var ns in suspiciousNamespaces)
            {
                if (code.Contains(ns, StringComparison.OrdinalIgnoreCase))
                {
                    int lineNo = code.Substring(0, code.IndexOf(ns, StringComparison.OrdinalIgnoreCase)).Count(c => c == '\n') + 1;
                    violations.Add(new Violation("Code Warning", $"Potential Rule 6.4 Violation: Using advanced namespace '{ns}' on line {lineNo}. Ensure only essential I/O functions are used."));
                }
            }

            // Check for specific forbidden methods that bypass algorithm requirements
            string[] forbiddenMethods = [".Sort(", ".Reverse(", "Math.Max(", "Math.Min(", "Math.Sqrt(", "Math.Pow("];
            foreach (var method in forbiddenMethods)
            {
                int index = code.IndexOf(method, StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                {
                    int lineNo = code.Substring(0, index).Count(c => c == '\n') + 1;
                    violations.Add(new Violation("Code Violation", $"Rule 6.4: Prohibited built-in method '{method}' found on line {lineNo}. Algorithms must be implemented manually."));
                }
            }


            // 3. Rule: No direct result output (Page 1, Rule 6.2)
            // Heuristic: Check for hardcoded result patterns without loops.
            // Use common control structures for both C# and VB
            string[] controlKeywords = ["for", "while", "do", "if"];
            bool hasControlStructure = false;

            foreach (var keyword in controlKeywords)
            {
                if (Regex.IsMatch(code, $@"\b{keyword}\b", RegexOptions.IgnoreCase))
                {
                    hasControlStructure = true;
                    break;
                }
            }

            if (!hasControlStructure)
            {
                violations.Add(new Violation("Code Violation", "Rule 6.2: Missing control structures (loops/if). Direct output of results is suspected."));
            }

        }
        catch (Exception ex)
        {
            violations.Add(new Violation("Code Error", $"Failed to read code: {ex.Message}"));
        }

        return (violations.Count == 0, violations);
    }
}
