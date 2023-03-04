// ReSharper disable HeapView.ObjectAllocation
namespace Pure.DI.Core.Models;

internal readonly record struct MdResolver(
    SemanticModel SemanticModel,
    SyntaxNode Source,
    int Position,
    ITypeSymbol ContractType,
    MdTag? Tag,
    ExpressionSyntax TargetValue)
{
    public override string ToString() => $"<=={ContractType}({Tag})";
}