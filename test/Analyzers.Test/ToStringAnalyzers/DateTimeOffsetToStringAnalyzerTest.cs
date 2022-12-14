using Microsoft.CodeAnalysis;
using MigrateToDocker.Analyzers.ToStringAnalyzers;
using Xunit;
using VerifyCS = MigrateToDocker.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    MigrateToDocker.Analyzers.ToStringAnalyzers.DateTimeOffsetToStringAnalyzer,
    MigrateToDocker.Analyzers.CodeFixes.DateTimeOffsetToStringAnalyzerCodeFixProvider>;

namespace MigrateToDocker.Analyzers.Test.ToStringAnalyzers
{
    public class DateTimeOffsetToStringAnalyzerTest
    {
        [Fact]
        public async Task NoCode_WithoutDiagnostics()
        {
            var test = string.Empty;

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [Fact]
        public async Task ToStringWithFormat_WithoutDiagnostics()
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
            DateTimeOffset dt = DateTimeOffset.Now;
            string str = dt.ToString(CultureInfo.InvariantCulture);
        }
    }
}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }


        [Fact]
        public async Task ParameterlessToString_WithoutNamespace_WithCodeFix()
        {
            var test = @"
using System;
namespace SomeApplication
{
    class {|#0:ClassName|}
    {
        void Main()
        {
            DateTimeOffset dt = DateTimeOffset.Now;
            string str = dt.ToString();
        }
    }
}";

            var fixTest = @"
using System;
using System.Globalization;

namespace SomeApplication
{
    class {|#0:ClassName|}
    {
        void Main()
        {
            DateTimeOffset dt = DateTimeOffset.Now;
            string str = dt.ToString(CultureInfo.InvariantCulture);
        }
    }
}";

            var expected = VerifyCS.Diagnostic(nameof(DateTimeOffsetToStringAnalyzer))
                .WithLocation(line: 10, column: 26)
                .WithArguments("ClassName")
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithMessage("Do not use parameterless ToString() method for culture-dependent types");

            await VerifyCS.VerifyCodeFixAsync(test, expected, fixTest);
        }

        [Fact]
        public async Task ParameterlessToString_WithExistingNamespace_WithCodeFix()
        {
            var test = @"
using System.Globalization;
using System;
namespace SomeApplication
{
    class {|#0:ClassName|}
    {
        void Main()
        {
            DateTimeOffset dt = DateTimeOffset.Now;
            string str = dt.ToString();
        }
    }
}";

            var fixTest = @"
using System.Globalization;
using System;
namespace SomeApplication
{
    class {|#0:ClassName|}
    {
        void Main()
        {
            DateTimeOffset dt = DateTimeOffset.Now;
            string str = dt.ToString(CultureInfo.InvariantCulture);
        }
    }
}";

            var expected = VerifyCS.Diagnostic(nameof(DateTimeOffsetToStringAnalyzer))
                .WithLocation(line: 11, column: 26)
                .WithArguments("ClassName")
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithMessage("Do not use parameterless ToString() method for culture-dependent types");

            await VerifyCS.VerifyCodeFixAsync(test, expected, fixTest);
        }
    }
}
