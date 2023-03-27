namespace Pure.DI.Core.CSharp;

internal class DisposeMethodBuilder: IBuilder<CompositionCode, CompositionCode>
{
    public CompositionCode Build(CompositionCode composition, CancellationToken cancellationToken)
    {
        var code = composition.Code;
        var membersCounter = composition.MembersCount;
        if (!composition.Singletons.Any())
        {
            return composition with { MembersCount = membersCounter };
        }
        
        if (composition.MembersCount > 0)
        {
            code.AppendLine();
        }

        code.AppendLine("public void Dispose()");
        code.AppendLine("{");
        using (code.Indent())
        {
            code.AppendLine($"lock ({Variable.DisposablesFieldName})");
            code.AppendLine("{");
            using (code.Indent())
            {
                if (composition.DisposableSingletonsCount > 0)
                {
                    code.AppendLine($"while ({Variable.DisposeIndexFieldName} > 0)");
                    code.AppendLine("{");
                    using (code.Indent())
                    {
                        code.AppendLine("try");
                        code.AppendLine("{");
                        using (code.Indent())
                        {
                            code.AppendLine($"{Variable.DisposablesFieldName}[--{Variable.DisposeIndexFieldName}].Dispose();");
                        }

                        code.AppendLine("}");
                        code.AppendLine("catch");
                        code.AppendLine("{");
                        using (code.Indent())
                        {
                            code.AppendLine("// ignored");
                        }

                        code.AppendLine("}");
                    }

                    code.AppendLine("}");
                    code.AppendLine();
                }

                foreach (var singletonField in composition.Singletons)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    code.AppendLine(
                        singletonField.Node.Type.IsValueType
                            ? $"{singletonField.Name}Created = false;"
                            : $"{singletonField.Name} = null;");
                }
            }

            code.AppendLine("}");
        }

        code.AppendLine("}");

        return composition with { MembersCount = membersCounter };
    }
}