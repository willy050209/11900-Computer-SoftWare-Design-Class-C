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

            var extension = (Path.GetExtension(codePath) ?? string.Empty).ToLowerInvariant();
            if (extension != ".cs" && extension != ".vb")
            {
                violations.Add(new Violation("System Error", $"Unsupported code file extension: {extension}. Only .cs and .vb are supported."));
                return (false, violations);
            }

            var code = File.ReadAllText(codePath);
            
            // Pre-process code to remove comments for loop detection
            string codeWithoutComments = Regex.Replace(code, @"//.*|/\*[\s\S]*?\*/|'.*", "");

            // 0. Rule: Required Header (Page 1, Rule 5.1)
            // Header format must be present for ALL questions 01-05:
            // // ******************************
            // // * 11900-9403xx Program Start *
            // // ******************************
            
            var missingHeaders = new List<string>();
            string commentSym = extension == ".vb" ? "'" : "//";
            for (int i = 1; i <= 5; i++)
            {
                string xx = i.ToString("D2");
                // Allow optional leading whitespace before the comment symbol
                string escapedSym = Regex.Escape(commentSym);
                string specificPattern = $@"\s*{escapedSym}\s*\*{{30}}\r?\n\s*{escapedSym}\s*\* 11900-9403{xx} Program Start \*\r?\n\s*{escapedSym}\s*\*{{30}}";
                if (!Regex.IsMatch(code, specificPattern))
                {
                    missingHeaders.Add(xx);
                }
            }


            if (missingHeaders.Count > 0)
            {
                violations.Add(new Violation("Code Violation", 
                    $"Rule 5.1: Missing or incorrect program headers for question(s): {string.Join(", ", missingHeaders)}.\n" +
                    "Each question section must start with:\n" +
                    $"{commentSym} ******************************\n" +
                    $"{commentSym} * 11900-9403xx Program Start *\n" +
                    $"{commentSym} ******************************"));
            }

            // 1. Rule: No 'Go To' (119003B14 Page 1, Rule 6.3)
            var gotoMatch = Regex.Match(codeWithoutComments, @"\bgoto\b", RegexOptions.IgnoreCase);
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
                if (codeWithoutComments.Contains(ns, StringComparison.OrdinalIgnoreCase))
                {
                    violations.Add(new Violation("Code Warning", $"Potential Rule 6.4 Violation: Using advanced namespace '{ns}'. Ensure only essential I/O functions are used."));
                }
            }

            // Check for specific forbidden methods that bypass algorithm requirements
            string[] forbiddenMethods = [".Sort(", ".Reverse(", "Math.Max(", "Math.Min(", "Math.Sqrt(", "Math.Pow("];
            foreach (var method in forbiddenMethods)
            {
                if (codeWithoutComments.Contains(method, StringComparison.OrdinalIgnoreCase))
                {
                    violations.Add(new Violation("Code Violation", $"Rule 6.4: Prohibited built-in method '{method}' found. Algorithms must be implemented manually."));
                }
            }


            // 3. Rule: No mixed loop types and no direct result output (Page 1, Rules 6.1 & 6.2)
            // The test draws a loop type (for, while, do while), and they cannot be mixed.
            bool hasFor = false;
            bool hasWhile = false;
            bool hasDo = false;

            if (extension == ".cs")
            {
                hasFor = Regex.IsMatch(codeWithoutComments, @"\bfor\b", RegexOptions.IgnoreCase);
                hasDo = Regex.IsMatch(codeWithoutComments, @"\bdo\b", RegexOptions.IgnoreCase);

                // Find all while and check if they are part of do-while.
                var whileMatches = Regex.Matches(codeWithoutComments, @"\bwhile\b", RegexOptions.IgnoreCase);
                foreach (Match m in whileMatches)
                {
                    string before = codeWithoutComments.Substring(0, m.Index);
                    // Check if 'while' is preceded by '}' (end of do block)
                    if (!Regex.IsMatch(before, @"\}\s*$", RegexOptions.IgnoreCase))
                    {
                        hasWhile = true;
                    }
                }
            }
            else // .vb
            {
                hasFor = Regex.IsMatch(codeWithoutComments, @"\bFor\b", RegexOptions.IgnoreCase);
                
                // VB While types: "While...End While" or "Do While...Loop"
                bool hasDoWhile = Regex.IsMatch(codeWithoutComments, @"\bDo\s+While\b", RegexOptions.IgnoreCase);
                bool hasStandaloneWhile = false;
                
                var whileMatches = Regex.Matches(codeWithoutComments, @"\bWhile\b", RegexOptions.IgnoreCase);
                foreach (Match m in whileMatches)
                {
                    string before = codeWithoutComments.Substring(0, m.Index);
                    // If 'While' is not preceded by 'Do ' or 'Loop ', it's a standalone While
                    if (!Regex.IsMatch(before, @"\bDo\s+$", RegexOptions.IgnoreCase) && 
                        !Regex.IsMatch(before, @"\bLoop\s+$", RegexOptions.IgnoreCase))
                    {
                        hasStandaloneWhile = true;
                    }
                }
                hasWhile = hasStandaloneWhile || hasDoWhile;

                // VB Do type: "Do...Loop While" or "Do...Loop Until" (strictly post-test)
                hasDo = Regex.IsMatch(codeWithoutComments, @"\bLoop\s+While\b", RegexOptions.IgnoreCase) || 
                        Regex.IsMatch(codeWithoutComments, @"\bLoop\s+Until\b", RegexOptions.IgnoreCase);
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
