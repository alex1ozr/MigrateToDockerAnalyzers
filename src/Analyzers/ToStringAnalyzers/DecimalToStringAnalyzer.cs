using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using MigrateToDocker.Analyzers.Abstract;

namespace MigrateToDocker.Analyzers.ToStringAnalyzers;

/// <summary>
/// Parameterless <see cref="decimal.ToString()"/> method usage analyzer
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DecimalToStringAnalyzer : ToStringAnalyzerBase
{
    public const string DiagnosticId = nameof(DecimalToStringAnalyzer);

    private static readonly DiagnosticDescriptor descriptor = new(
        DiagnosticId,
        LocalizableStrings.ToStringAnalyzer.Title,
        LocalizableStrings.ToStringAnalyzer.MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        LocalizableStrings.ToStringAnalyzer.Description);

    /// <inheritdoc/>
    protected override DiagnosticDescriptor DiagnosticDescriptor => descriptor;

    /// <inheritdoc/>
    protected override string TargetMemberSymbol => "decimal.ToString()";
}
