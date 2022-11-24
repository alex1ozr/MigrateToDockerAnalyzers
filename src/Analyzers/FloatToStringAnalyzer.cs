using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using MigrateToDocker.Analyzers.Abstract;

namespace MigrateToDocker.Analyzers;

/// <summary>
/// Parameterless <see cref="float.ToString()"/> method usage analyzer
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class FloatToStringAnalyzer : ToStringAnalyzerBase
{
    public const string DiagnosticId = nameof(FloatToStringAnalyzer);

    private static readonly DiagnosticDescriptor descriptor = new(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        Description);

    /// <inheritdoc/>
    protected override DiagnosticDescriptor DiagnosticDescriptor => descriptor;

    /// <inheritdoc/>
    protected override string MethodSymbol => "float.ToString()";
}
