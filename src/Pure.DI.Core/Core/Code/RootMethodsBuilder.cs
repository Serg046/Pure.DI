// ReSharper disable ClassNeverInstantiated.Global
namespace Pure.DI.Core.Code;

internal sealed class RootMethodsBuilder: IBuilder<CompositionCode, CompositionCode>
{
    private readonly IBuildTools _buildTools;
    private static readonly string[] NewLineSeparators = { Environment.NewLine };

    public RootMethodsBuilder(IBuildTools buildTools) => 
        _buildTools = buildTools;

    public CompositionCode Build(CompositionCode composition)
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

        var generatePrivateRoots = composition.Source.Source.Hints.GetHint(Hint.Resolve, SettingState.On) == SettingState.On;
        var membersCounter = composition.MembersCount;
        code.AppendLine("#region Composition Roots");
        var isFirst = true;
        foreach (var root in composition.Roots.Where(i => generatePrivateRoots || i.IsPublic))
        {
            if (isFirst)
            {
                isFirst = false;
            }
            else
            {
                code.AppendLine();
            }

            BuildRoot(composition, root.Injection.Type, root);
            membersCounter++;
        }
        code.AppendLine("#endregion");
        return composition with { MembersCount = membersCounter };
    }
    
    private void BuildRoot(CompositionCode composition, ITypeSymbol type, Root root)
    {
        var code = composition.Code;
        var isMethod = !root.Args.IsEmpty || (root.Kind & RootKinds.Method) == RootKinds.Method;
        var rootArgsStr = "";
        if (isMethod)
        {
            isMethod = true;
            rootArgsStr = $"({string.Join(", ", root.Args.Select(arg => $"{arg.InstanceType} {arg.VariableName}"))})";
        }

        if (isMethod)
        {
            _buildTools.AddPureHeader(code);
        }

        var isStatic = (root.Kind & RootKinds.Static) == RootKinds.Static;
        var isPartial = (root.Kind & RootKinds.Partial) == RootKinds.Partial;
        var name = new StringBuilder();
        name.Append(root.IsPublic ? "public" : "private");
        if (isStatic)
        {
            name.Append(" static");
        }

        if (isPartial)
        {
            name.Append(" partial");
        }
        
        name.Append(' ');
        name.Append(type);
        
        name.Append(' ');
        name.Append(root.PropertyName);
        
        name.Append(rootArgsStr);
        
        code.AppendLine(name.ToString());
        code.AppendLine("{");
        using (code.Indent())
        {
            var indentToken = Disposables.Empty;
            if (!isMethod)
            {
                _buildTools.AddPureHeader(code);
                code.AppendLine("get");
                code.AppendLine("{");
                indentToken = code.Indent();
            }

            try
            {
                if (composition.Source.Source.Hints.GetHint(Hint.FormatCode) == SettingState.On)
                {
                    var codeText = string.Join(Environment.NewLine, root.Lines);
                    var syntaxTree = CSharpSyntaxTree.ParseText(codeText);
                    codeText = syntaxTree.GetRoot().NormalizeWhitespace().ToString();
                    var lines = codeText.Split(NewLineSeparators, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        code.AppendLine(line);
                    }
                }
                else
                {
                    code.AppendLines(root.Lines);
                }
            }
            finally
            {
                indentToken.Dispose();
            }

            if (!isMethod)
            {
                code.AppendLine("}");
            }
        }

        code.AppendLine("}");
    }
}