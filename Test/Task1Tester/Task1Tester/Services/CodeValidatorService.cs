// filepath: Services/CodeValidatorService.cs
namespace Task1Tester.Services;

public static class CodeValidatorService
{
    /// <summary>
    /// Validates C# source code against 119003B14 Level C requirements.
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

            var code = File.ReadAllText(codePath);

            // 1. Rule: No 'Go To' (119003B14 Page 1, Rule 6.3)
            if (Regex.IsMatch(code, @"\bgoto\b", RegexOptions.IgnoreCase))
            {
                violations.Add(new Violation("Code Violation", "Rule 6.3: Use of 'Go To' is strictly prohibited."));
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
                if (code.Contains(ns))
                    violations.Add(new Violation("Code Warning", $"Potential Rule 6.4 Violation: Using advanced namespace '{ns}'. Ensure only essential I/O functions are used."));
            }

            // Check for specific forbidden methods that bypass algorithm requirements
            string[] forbiddenMethods = [".Sort(", ".Reverse(", "Math.Max(", "Math.Min(", "Math.Sqrt(", "Math.Pow("];
            foreach (var method in forbiddenMethods)
            {
                if (code.Contains(method))
                    violations.Add(new Violation("Code Violation", $"Rule 6.4: Prohibited built-in method '{method}' found. Algorithms must be implemented manually."));
            }

            // 3. Rule: No direct result output (Page 1, Rule 6.2)
            // Heuristic: Check for hardcoded result patterns without loops.
            string[] resultKeywords = ["palindrome", "prime number", "BMI值", "矩陣相加"];
            bool hasControlStructure = code.Contains("for") || code.Contains("while") || code.Contains("do") || code.Contains("if");

            if (!hasControlStructure)
            {
                violations.Add(new Violation("Code Violation", "Rule 6.2: Missing control structures (loops/if). Direct output of results is suspected."));
            }

            // 4. Rule: Primary Constructors and Modern Syntax (Internal Standard)
            // We encourage clean code, but strictly follow exam rules for failure.

        }
        catch (Exception ex)
        {
            violations.Add(new Violation("Code Error", $"Failed to read code: {ex.Message}"));
        }

        return (violations.Count == 0, violations);
    }
}
