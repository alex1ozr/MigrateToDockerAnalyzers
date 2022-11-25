using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using MigrateToDocker.Analyzers.Abstract;

namespace MigrateToDocker.Analyzers.ParseAnalyzers;

/// <summary>
/// Parameterless <see cref="double.Parse(string)"/> or <see cref="double.TryParse(string?, out double)"/>
/// method usage analyzer
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DoubleParseAnalyzer : ParseAnalyzerBase
{
    public const string DiagnosticId = nameof(DoubleParseAnalyzer);

    private static readonly DiagnosticDescriptor descriptor = new(
        DiagnosticId,
        LocalizableStrings.ParseAnalyzer.Title,
        LocalizableStrings.ParseAnalyzer.MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        LocalizableStrings.ParseAnalyzer.Description);

    private static readonly string[] methodSymbols =
        { "double.Parse(string)", "double.TryParse(string?, out double)" };

    /// <inheritdoc/>
    protected override DiagnosticDescriptor DiagnosticDescriptor => descriptor;

    /// <inheritdoc/>
    protected override IReadOnlyCollection<string> TargetMemberSymbols => methodSymbols;
}
