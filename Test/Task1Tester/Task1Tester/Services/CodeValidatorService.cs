// filepath: Services/CodeValidatorService.cs
namespace Task1Tester.Services;

public static class CodeValidatorService
{
    /// <summary>
    /// Validates source code (C# or VB.NET) against 119003B14 Level C requirements.
    /// </summary>
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

            var extension = Path.GetExtension(codePath).ToLowerInvariant();
            if (extension != ".cs" && extension != ".vb")
            {
                violations.Add(new Violation("System Error", $"Unsupported code file extension: {extension}. Only .cs and .vb are supported."));
                return (false, violations);
            }

            var code = File.ReadAllText(codePath);

            // 0. Rule: Required Header (Page 1, Rule 5.1)
            // Header format:
            // ******************************
            // * 11900-940304 Program Start *
            // ******************************
            // where xx is 01, 02, 03, 04, or 05
            var lines = code.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);
            bool headerValid = false;
            if (lines.Length >= 3)
            {
                string line1 = lines[0].Trim();
                string line2 = lines[1].Trim();
                string line3 = lines[2].Trim();
                
                headerValid = line1 == "// ******************************" &&
                             Regex.IsMatch(line2, @"^// \* 11900-94030[1-5] Program Start \*$") &&
                             line3 == "// ******************************";
            }

            if (!headerValid)
            {
                violations.Add(new Violation("Code Violation", 
                    "Rule 5.1: Missing or incorrect program header at the beginning of the file.\n" +
                    "Expected exactly:\n" +
                    "// ******************************\n" +
                    "// * 11900-9403xx Program Start *\n" +
                    "// ******************************\n" +
                    "where xx is 01, 02, 03, 04, or 05."));
            }

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


            // 3. Rule: No mixed loop types and no direct result output (Page 1, Rules 6.1 & 6.2)
            // The test draws a loop type (for, while, do while), and they cannot be mixed.
            bool hasFor = Regex.IsMatch(code, @"\bfor\b", RegexOptions.IgnoreCase);
            bool hasWhile = false;
            bool hasDo = Regex.IsMatch(code, @"\bdo\b", RegexOptions.IgnoreCase);

            if (extension == ".cs")
            {
                // In C#, while can be standalone or part of do-while.
                // We count it as standalone if it's not preceded by '}' within a few chars (heuristic).
                // Better: find all while and check if they are part of do-while.
                var whileMatches = Regex.Matches(code, @"\bwhile\b", RegexOptions.IgnoreCase);
                foreach (Match m in whileMatches)
                {
                    // Look backwards for 'do' that isn't closed (simplified heuristic)
                    string before = code.Substring(0, m.Index);
                    if (!before.TrimEnd().EndsWith("}")) 
                    {
                        // Check if it's potentially a standalone while(condition)
                        if (Regex.IsMatch(before, @"\bdo\b\s*\{", RegexOptions.IgnoreCase | RegexOptions.RightToLeft))
                        {
                            // This while might be part of do-while. 
                            // If we already have 'do' detected, we don't count this as a separate 'while' type.
                        }
                        else
                        {
                            hasWhile = true;
                        }
                    }
                }
            }
            else // .vb
            {
                // VB: 'Do While', 'Do Until', 'While', 'For'
                hasWhile = Regex.IsMatch(code, @"\bWhile\b", RegexOptions.IgnoreCase) && !Regex.IsMatch(code, @"\bDo\s+While\b", RegexOptions.IgnoreCase);
                // Note: VB 'While' is distinct from 'Do While'
            }

            var usedLoops = new List<string>();
            if (hasFor) usedLoops.Add("for");
            if (hasWhile) usedLoops.Add("while");
            if (hasDo) usedLoops.Add("do");

            // Check against expected loop type
            if (usedLoops.Count > 0)
            {
                bool onlyExpected = usedLoops.All(l => l == expectedLoopType);
                if (!onlyExpected || usedLoops.Count > 1)
                {
                    violations.Add(new Violation("Code Violation", $"Rule Violation: Mixed or incorrect loop type detected. The drawn type is '{expectedLoopType}', but you used: {string.Join(", ", usedLoops)}."));
                }
            }
            else
            {
                violations.Add(new Violation("Code Violation", $"Rule 6.2: Missing required loop control structures. Expected a '{expectedLoopType}' loop. Direct output of results is prohibited."));
            }

        }
        catch (Exception ex)
        {
            violations.Add(new Violation("Code Error", $"Failed to read code: {ex.Message}"));
        }

        return (violations.Count == 0, violations);
    }
}
