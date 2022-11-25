using Microsoft.CodeAnalysis;
using MigrateToDocker.Analyzers.ParseAnalyzers;
using Xunit;
using VerifyCS = MigrateToDocker.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<
    MigrateToDocker.Analyzers.ParseAnalyzers.DateTimeParseAnalyzer>;

namespace MigrateToDocker.Analyzers.Test.ParseAnalyzers
{
    public class DateTimeParseAnalyzerTest
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
            string str = ""01.01.2000"";
            DateTime dt = DateTime.Parse(str, CultureInfo.InvariantCulture);
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
            string str = ""01.01.2000"";
            DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt);
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
            string str = ""01.01.2000"";
            DateTime dt = DateTime.Parse(str);
        }
    }
}";

            var expected = VerifyCS.Diagnostic(nameof(DateTimeParseAnalyzer))
                .WithLocation(line: 10, column: 27)
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
            string str = ""01.01.2000"";
            DateTime.TryParse(str, out var dt);
        }
    }
}";

            var expected = VerifyCS.Diagnostic(nameof(DateTimeParseAnalyzer))
                .WithLocation(line: 10, column: 13)
                .WithArguments("ClassName")
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithMessage("Do not use Parse() method without format or culture argument for culture dependent types");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }
}
