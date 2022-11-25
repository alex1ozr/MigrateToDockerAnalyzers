using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using MigrateToDocker.Analyzers.Abstract;

namespace MigrateToDocker.Analyzers.ParseAnalyzers;

/// <summary>
/// Parameterless <see cref="float.Parse(string)"/> or <see cref="float.TryParse(string?, out float)"/>
/// method usage analyzer
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class FloatParseAnalyzer : ParseAnalyzerBase
{
    public const string DiagnosticId = nameof(FloatParseAnalyzer);

    private static readonly DiagnosticDescriptor descriptor = new(
        DiagnosticId,
        LocalizableStrings.ParseAnalyzer.Title,
        LocalizableStrings.ParseAnalyzer.MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        LocalizableStrings.ParseAnalyzer.Description);

    private static readonly string[] methodSymbols =
        { "float.Parse(string)", "float.TryParse(string?, out float)" };

    /// <inheritdoc/>
    protected override DiagnosticDescriptor DiagnosticDescriptor => descriptor;

    /// <inheritdoc/>
    protected override IReadOnlyCollection<string> TargetMemberSymbols => methodSymbols;
}
