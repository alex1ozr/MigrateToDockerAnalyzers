using Microsoft.CodeAnalysis;
using MigrateToDocker.Analyzers.ParseAnalyzers;
using Xunit;
using VerifyCS = MigrateToDocker.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<
    MigrateToDocker.Analyzers.ParseAnalyzers.DoubleParseAnalyzer>;

namespace MigrateToDocker.Analyzers.Test.ParseAnalyzers
{
    public class DoubleParseAnalyzerTest
    {
        [Fact]
        public async Task NoCode_WithoutDiagnostics()
        {
            var test = string.Empty;

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [Fact]
        public async Task ParseWithFormat_WithoutDiagnostics()
        {
            var test = @"
using System;
using System.Globalization;
namespace SomeApplication
{
    class {|#0:ClassName|}
    {
        void Main()
        {
            string str = ""1.5"";
            double num = Double.Parse(str, CultureInfo.InvariantCulture);
        }
    }
}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
        
        [Fact]
        public async Task TryParseWithFormat_WithoutDiagnostics()
        {
            var test = @"
using System;
using System.Globalization;
namespace SomeApplication
{
    class {|#0:ClassName|}
    {
        void Main()
        {
            string str = ""1.5"";
            Double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var num);
        }
    }
}";
            
            await VerifyCS.VerifyAnalyzerAsync(test);
        }


        [Fact]
        public async Task ParseWithoutCulture_WithDiagnostics()
        {
            var test = @"
using System;
namespace SomeApplication
{
    class {|#0:ClassName|}
    {
        void Main()
        {
            string str = ""1.5"";
            double num = Double.Parse(str);
        }
    }
}";

            var expected = VerifyCS.Diagnostic(nameof(DoubleParseAnalyzer))
                .WithLocation(line: 10, column: 26)
                .WithArguments("ClassName")
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithMessage("Do not use Parse() method without format or culture argument for culture dependent types");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
        
        [Fact]
        public async Task TryParseWithoutCulture_WithDiagnostics()
        {
            var test = @"
using System;
namespace SomeApplication
{
    class {|#0:ClassName|}
    {
        void Main()
        {
            string str = ""1.5"";
            Double.TryParse(str, out var num);
        }
    }
}";

            var expected = VerifyCS.Diagnostic(nameof(DoubleParseAnalyzer))
                .WithLocation(line: 10, column: 13)
                .WithArguments("ClassName")
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithMessage("Do not use Parse() method without format or culture argument for culture dependent types");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }
}
