using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis;
using System.Composition;
using MigrateToDocker.Analyzers.CodeFixes.Abstract;
using MigrateToDocker.Analyzers.ToStringAnalyzers;

namespace MigrateToDocker.Analyzers.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DateTimeToStringAnalyzerCodeFixProvider)), Shared]
public class DateTimeToStringAnalyzerCodeFixProvider : ToStringAnalyzerCodeFixProviderBase
{
    public override string DiagnosticId => DateTimeToStringAnalyzer.DiagnosticId;
}
