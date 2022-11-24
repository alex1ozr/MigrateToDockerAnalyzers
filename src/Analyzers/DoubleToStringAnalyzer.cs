using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using MigrateToDocker.Analyzers.Abstract;

namespace MigrateToDocker.Analyzers;

/// <summary>
/// Parameterless <see cref="double.ToString()"/> method usage analyzer
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DoubleToStringAnalyzer : ToStringAnalyzerBase
{
    public const string DiagnosticId = nameof(DoubleToStringAnalyzer);

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
    protected override string MethodSymbol => "double.ToString()";
}
