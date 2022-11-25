using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using MigrateToDocker.Analyzers.Abstract;

namespace MigrateToDocker.Analyzers.ParseAnalyzers;

/// <summary>
/// Parameterless <see cref="decimal.Parse(string)"/> or <see cref="decimal.TryParse(string?, out decimal)"/>
/// method usage analyzer
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DecimalParseAnalyzer : ParseAnalyzerBase
{
    public const string DiagnosticId = nameof(DecimalParseAnalyzer);

    private static readonly DiagnosticDescriptor descriptor = new(
        DiagnosticId,
        LocalizableStrings.ParseAnalyzer.Title,
        LocalizableStrings.ParseAnalyzer.MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        LocalizableStrings.ParseAnalyzer.Description);

    private static readonly string[] methodSymbols =
        { "decimal.Parse(string)", "decimal.TryParse(string?, out decimal)" };

    /// <inheritdoc/>
    protected override DiagnosticDescriptor DiagnosticDescriptor => descriptor;

    /// <inheritdoc/>
    protected override IReadOnlyCollection<string> TargetMemberSymbols => methodSymbols;
}
