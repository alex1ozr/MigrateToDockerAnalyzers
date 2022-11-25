using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace MigrateToDocker.Analyzers.Abstract;

/// <summary>
/// Parameterless Parse() method usage analyzer
/// </summary>
public abstract class ParseAnalyzerBase : DiagnosticAnalyzer
{
    private static readonly HashSet<string> targetMethods = new() { "Parse", "TryParse" };

    /// <summary>
    /// Analyzer Category
    /// </summary>
    protected static readonly string Category = "Usage";

    /// <summary>
    /// <see cref="DiagnosticDescriptor"/>
    /// </summary>
    protected abstract DiagnosticDescriptor DiagnosticDescriptor { get; }

    /// <summary>
    /// Member symbols to look for
    /// </summary>
    protected abstract IReadOnlyCollection<string> TargetMemberSymbols { get; }

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => ImmutableArray.Create(DiagnosticDescriptor);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.InvocationExpression);
    }

    private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
    {
        var invocationExpression = (InvocationExpressionSyntax)context.Node;
        var memberAccessExpression = invocationExpression.Expression as MemberAccessExpressionSyntax;
        var memberAccessName = memberAccessExpression?.Name?.ToString();

        if (string.IsNullOrEmpty(memberAccessName) || !targetMethods.Contains(memberAccessName!))
        {
            return;
        }

        var memberSymbol = context.SemanticModel
            .GetSymbolInfo(invocationExpression)
            .Symbol?.ToString();

        if (string.IsNullOrEmpty(memberSymbol))
        {
            return;
        }

        if (!TargetMemberSymbols.Any(x => memberSymbol!.StartsWith(x)))
        {
            return;
        }

        var diagnostic = Diagnostic.Create(DiagnosticDescriptor, context.Node.GetLocation());
        context.ReportDiagnostic(diagnostic);
    }
}
