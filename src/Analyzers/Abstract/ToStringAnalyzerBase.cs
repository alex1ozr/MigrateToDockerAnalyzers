using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace MigrateToDocker.Analyzers.Abstract;

/// <summary>
/// Parameterless ToString() method usage analyzer
/// </summary>
public abstract class ToStringAnalyzerBase : DiagnosticAnalyzer
{
    /// <summary>
    /// Analyzer Category
    /// </summary>
    protected static readonly string Category = "Usage";

    /// <summary>
    /// <see cref="DiagnosticDescriptor"/>
    /// </summary>
    protected abstract DiagnosticDescriptor DiagnosticDescriptor { get; }

    /// <summary>
    /// Member symbol to look for
    /// </summary>
    protected abstract string TargetMemberSymbol { get; }

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

        if (memberAccessExpression?.Name?.ToString() != nameof(object.ToString))
        {
            return;
        }

        var memberSymbol = context.SemanticModel
            .GetSymbolInfo(invocationExpression)
            .Symbol?.ToString();

        if (memberSymbol == null || !memberSymbol.StartsWith(TargetMemberSymbol))
        {
            return;
        }

        var diagnostic = Diagnostic.Create(DiagnosticDescriptor, context.Node.GetLocation());
        context.ReportDiagnostic(diagnostic);
    }
}
