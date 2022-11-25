using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using MigrateToDocker.Analyzers.Abstract;

namespace MigrateToDocker.Analyzers.ParseAnalyzers;

/// <summary>
/// Parameterless <see cref="DateTime.Parse(string)"/> or <see cref="DateTime.TryParse(string?, out DateTime)"/>
/// method usage analyzer
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DateTimeParseAnalyzer : ParseAnalyzerBase
{
    public const string DiagnosticId = nameof(DateTimeParseAnalyzer);

    private static readonly DiagnosticDescriptor descriptor = new(
        DiagnosticId,
        LocalizableStrings.ParseAnalyzer.Title,
        LocalizableStrings.ParseAnalyzer.MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        LocalizableStrings.ParseAnalyzer.Description);

    private static readonly string[] memberSymbols =
        { "System.DateTime.Parse(string)", "System.DateTime.TryParse(string?, out System.DateTime)" };

    /// <inheritdoc/>
    protected override DiagnosticDescriptor DiagnosticDescriptor => descriptor;

    /// <inheritdoc/>
    protected override IReadOnlyCollection<string> TargetMemberSymbols => memberSymbols;
}
