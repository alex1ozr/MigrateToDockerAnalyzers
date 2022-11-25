using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using MigrateToDocker.Analyzers.Abstract;

namespace MigrateToDocker.Analyzers.ToStringAnalyzers;

/// <summary>
/// Parameterless <see cref="DateTime.ToString()"/> method usage analyzer
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DateTimeToStringAnalyzer : ToStringAnalyzerBase
{
    public const string DiagnosticId = nameof(DateTimeToStringAnalyzer);

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
    protected override string TargetMemberSymbol => "System.DateTime.ToString()";
}
