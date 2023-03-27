namespace Pure.DI.Core.CSharp;

internal class RootPropertiesBuilder: IBuilder<CompositionCode, CompositionCode>
{
    public CompositionCode Build(CompositionCode composition, CancellationToken cancellationToken)
    {
        if (!composition.Roots.Any())
        {
            return composition;
        }
        
        var code = composition.Code;
        if (composition.MembersCount > 0)
        {
            code.AppendLine();
        }
        
        var membersCounter = composition.MembersCount;
        var roots = composition.Roots
            .OrderByDescending(i => i.IsPublic)
            .ThenBy(i => i.Node.Binding.Id)
            .ThenBy(i => i.PropertyName);

        code.AppendLine("#region Roots");
        var isFirst = true;
        foreach (var root in roots)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (isFirst)
            {
                isFirst = false;
            }
            else
            {
                code.AppendLine();
            }

            code.AppendLines(BuildProperty(root.Injection.Type, root));
            membersCounter++;
        }
        code.AppendLine("#endregion");
        return composition with { MembersCount = membersCounter };
    }
    
    private static ImmutableArray<Line> BuildProperty(ITypeSymbol type, Root root)
    {
        var code = new LinesBuilder();
        code.AppendLine($"{(root.IsPublic ? "public" : "private")} {type} {root.PropertyName}");
        code.AppendLine("{");
        using (code.Indent())
        {
            code.AppendLine(CodeExtensions.MethodImplOptions);
            code.AppendLine("get");
            code.AppendLine("{");
            using (code.Indent())
            {
                code.AppendLines(root.Lines);
            }

            code.AppendLine("}");
        }

        code.AppendLine("}");
        return code.Lines.ToImmutableArray();
    }
}