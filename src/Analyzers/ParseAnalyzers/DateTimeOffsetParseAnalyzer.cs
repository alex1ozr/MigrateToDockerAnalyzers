using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using MigrateToDocker.Analyzers.Abstract;

namespace MigrateToDocker.Analyzers.ParseAnalyzers;

/// <summary>
/// Parameterless <see cref="DateTimeOffset.Parse(string)"/> or <see cref="DateTimeOffset.TryParse(string?, out DateTimeOffset)"/>
/// method usage analyzer
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DateTimeOffsetParseAnalyzer : ParseAnalyzerBase
{
    public const string DiagnosticId = nameof(DateTimeOffsetParseAnalyzer);

    private static readonly DiagnosticDescriptor descriptor = new(
        DiagnosticId,
        LocalizableStrings.ParseAnalyzer.Title,
        LocalizableStrings.ParseAnalyzer.MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        LocalizableStrings.ParseAnalyzer.Description);

    private static readonly string[] memberSymbols =
        { "System.DateTimeOffset.Parse(string)", "System.DateTimeOffset.TryParse(string?, out System.DateTimeOffset)" };

    /// <inheritdoc/>
    protected override DiagnosticDescriptor DiagnosticDescriptor => descriptor;

    /// <inheritdoc/>
    protected override IReadOnlyCollection<string> TargetMemberSymbols => memberSymbols;
}
