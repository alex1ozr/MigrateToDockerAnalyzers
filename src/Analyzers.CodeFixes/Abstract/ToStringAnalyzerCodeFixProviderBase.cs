using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using System.Globalization;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace MigrateToDocker.Analyzers.CodeFixes;

public abstract class ToStringAnalyzerCodeFixProviderBase : CodeFixProvider
{
    public abstract string DiagnosticId { get; }

    public sealed override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(DiagnosticId); }
    }

    public sealed override FixAllProvider GetFixAllProvider()
    {
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
        return WellKnownFixAllProviders.BatchFixer;
    }

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        var argument = root.FindNode(diagnosticSpan);

        // Find the type declaration identified by the diagnostic.
        var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<InvocationExpressionSyntax>().First();

        // Register a code action that will invoke the fix.
        context.RegisterCodeFix(
            CodeAction.Create(
                title: CodeFixResources.ToStringCodeFix_Title,
                createChangedDocument: c => FixAsync(context.Document, declaration, c),
                equivalenceKey: nameof(CodeFixResources.ToStringCodeFix_Title)),
            diagnostic);
    }

    private async Task<Document> FixAsync(Document document, InvocationExpressionSyntax invocationExpr, CancellationToken cancellationToken)
    {
        // Create a new list of arguments with CultureInfo.InvariantCulture
        var arguments = invocationExpr.ArgumentList.AddArguments(
            Argument(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("CultureInfo"), IdentifierName("InvariantCulture"))));

        // Indicate to format the list with the current coding style
        var formattedLocal = arguments.WithAdditionalAnnotations(Formatter.Annotation);

        // Replace the old local declaration with the new local declaration.
        var oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var newRoot = oldRoot.ReplaceNode(invocationExpr.ArgumentList, formattedLocal);

        newRoot = FixUsings(newRoot);

        return document.WithSyntaxRoot(newRoot);
    }

    private static SyntaxNode FixUsings(SyntaxNode root)
    {
        var systemGlobalizationUsingStatement =
            UsingDirective(QualifiedName(IdentifierName("System"), IdentifierName("Globalization")));

        var compilation = root as CompilationUnitSyntax;

        if (null == compilation)
        {
            root = root.InsertNodesBefore(
                    root.ChildNodes().First(),
                    new[] { systemGlobalizationUsingStatement });
        }
        else if (compilation.Usings.All(u => u.Name.GetText().ToString() != typeof(CultureInfo).Namespace))
        {
            root = root.InsertNodesAfter(
                compilation.Usings.Last(),
                new[] { systemGlobalizationUsingStatement });
        }
        
        return root;
    }
}
