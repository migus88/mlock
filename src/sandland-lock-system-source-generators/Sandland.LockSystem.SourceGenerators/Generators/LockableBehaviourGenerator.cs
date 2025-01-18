using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sandland.LockSystem.SourceGenerators.Generators;

[Generator]
public class LockableBehaviourGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications();
    }

    public void Execute(GeneratorExecutionContext context)
    {
        throw new NotImplementedException();
    }

    private class SyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> Behaviours { get; } = new();
        
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {

            if (syntaxNode is not ClassDeclarationSyntax { AttributeLists.Count: > 0 } typeDeclarationSyntax)
            {
                return;
            }
        
            if (!typeDeclarationSyntax.AttributeLists.Any(l => l.Attributes.Any(a => a.Name.ToString().EndsWith(typeof()))))
            {
                return false;
            }
        
            convertedValue = typeDeclarationSyntax;
            return true;
        }
    }
}