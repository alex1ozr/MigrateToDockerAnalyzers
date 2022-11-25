using Microsoft.CodeAnalysis;
using MigrateToDocker.Analyzers.ParseAnalyzers;
using Xunit;
using VerifyCS = MigrateToDocker.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<
    MigrateToDocker.Analyzers.ParseAnalyzers.DecimalParseAnalyzer>;

namespace MigrateToDocker.Analyzers.Test.ParseAnalyzers
{
    public class DecimalParseAnalyzerTest
    {
        [Fact]
        public async Task NoCode_WithoutDiagnostics()
        {
            var test = string.Empty;

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [Theory]
        [MemberData(nameof(TestData.DecimalType), MemberType = typeof(TestData))]
        public async Task ParseWithFormat_WithoutDiagnostics(string decimalType)
        {
            var test = $@"
using System;
using System.Globalization;
namespace SomeApplication
{{
    class {{|#0:ClassName|}}
    {{
        void Main()
        {{
            string str = ""1.5"";
            decimal num = {decimalType}.Parse(str, CultureInfo.InvariantCulture);
        }}
    }}
}}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
        
        [Theory]
        [MemberData(nameof(TestData.DecimalType), MemberType = typeof(TestData))]
        public async Task TryParseWithFormat_WithoutDiagnostics(string decimalType)
        {
            var test = $@"
using System;
using System.Globalization;
namespace SomeApplication
{{
    class {{|#0:ClassName|}}
    {{
        void Main()
        {{
            string str = ""1.5"";
            {decimalType}.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var num);
        }}
    }}
}}";
            
            await VerifyCS.VerifyAnalyzerAsync(test);
        }
        
        [Theory]
        [MemberData(nameof(TestData.DecimalType), MemberType = typeof(TestData))]
        public async Task ParseWithoutCulture_WithDiagnostics(string decimalType)
        {
            var test = $@"
using System;
namespace SomeApplication
{{
    class {{|#0:ClassName|}}
    {{
        void Main()
        {{
            string str = ""1.5"";
            decimal num = {decimalType}.Parse(str);
        }}
    }}
}}";

            var expected = VerifyCS.Diagnostic(nameof(DecimalParseAnalyzer))
                .WithLocation(line: 10, column: 27)
                .WithArguments("ClassName")
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithMessage("Do not use Parse() method without format or culture argument for culture-dependent types");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
        
        [Theory]
        [MemberData(nameof(TestData.DecimalType), MemberType = typeof(TestData))]
        public async Task TryParseWithoutCulture_WithDiagnostics(string decimalType)
        {
            var test = $@"
using System;
namespace SomeApplication
{{
    class {{|#0:ClassName|}}
    {{
        void Main()
        {{
            string str = ""1.5"";
            {decimalType}.TryParse(str, out var num);
        }}
    }}
}}";

            var expected = VerifyCS.Diagnostic(nameof(DecimalParseAnalyzer))
                .WithLocation(line: 10, column: 13)
                .WithArguments("ClassName")
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithMessage("Do not use Parse() method without format or culture argument for culture-dependent types");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }
}
