using Microsoft.CodeAnalysis;
using MigrateToDocker.Analyzers.ParseAnalyzers;
using Xunit;
using VerifyCS = MigrateToDocker.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<
    MigrateToDocker.Analyzers.ParseAnalyzers.FloatParseAnalyzer>;

namespace MigrateToDocker.Analyzers.Test.ParseAnalyzers
{
    public class FloatParseAnalyzerTest
    {
        [Fact]
        public async Task NoCode_WithoutDiagnostics()
        {
            var test = string.Empty;

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [Theory]
        [MemberData(nameof(TestData.FloatType), MemberType = typeof(TestData))]
        public async Task ParseWithFormat_WithoutDiagnostics(string floatType)
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
            float num = {floatType}.Parse(str, CultureInfo.InvariantCulture);
        }}
    }}
}}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
        
        [Theory]
        [MemberData(nameof(TestData.FloatType), MemberType = typeof(TestData))]
        public async Task TryParseWithFormat_WithoutDiagnostics(string floatType)
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
            {floatType}.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var num);
        }}
    }}
}}";
            
            await VerifyCS.VerifyAnalyzerAsync(test);
        }


        [Theory]
        [MemberData(nameof(TestData.FloatType), MemberType = typeof(TestData))]
        public async Task ParseWithoutCulture_WithDiagnostics(string floatType)
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
            float num = {floatType}.Parse(str);
        }}
    }}
}}";

            var expected = VerifyCS.Diagnostic(nameof(FloatParseAnalyzer))
                .WithLocation(line: 10, column: 25)
                .WithArguments("ClassName")
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithMessage("Do not use Parse() method without format or culture argument for culture-dependent types");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
        
        [Theory]
        [MemberData(nameof(TestData.FloatType), MemberType = typeof(TestData))]
        public async Task TryParseWithoutCulture_WithDiagnostics(string floatType)
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
            {floatType}.TryParse(str, out var num);
        }}
    }}
}}";

            var expected = VerifyCS.Diagnostic(nameof(FloatParseAnalyzer))
                .WithLocation(line: 10, column: 13)
                .WithArguments("ClassName")
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithMessage("Do not use Parse() method without format or culture argument for culture-dependent types");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }
}
