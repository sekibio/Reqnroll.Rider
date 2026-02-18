using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Application;
using JetBrains.Application.Parts;
using ReSharperPlugin.ReqnrollRiderPlugin.Psi;

namespace ReSharperPlugin.ReqnrollRiderPlugin.Utils.Steps;

[ShellComponent(Instantiation.DemandAnyThreadUnsafe)]
public class StepScaffoldingGenerator : IStepScaffoldingGenerator
{
    private readonly IStepDefinitionBuilder _stepDefinitionBuilder;

    public StepScaffoldingGenerator(IStepDefinitionBuilder stepDefinitionBuilder)
    {
        _stepDefinitionBuilder = stepDefinitionBuilder;
    }

    public string GenerateScaffoldingCode(
        GherkinStepKind stepKind,
        string stepText,
        CultureInfo culture,
        bool isAsync,
        bool hasMultilineParameter = false,
        bool hasTableParameter = false)
    {
        // Get method name from step text
        var methodName = _stepDefinitionBuilder.GetStepDefinitionMethodNameFromStepText(stepKind, stepText, culture);

        // Get pattern for the step
        var pattern = _stepDefinitionBuilder.GetPattern(stepText, culture);

        // Get parameters
        var parameters = _stepDefinitionBuilder.GetStepDefinitionParameters(stepText, culture).ToList();

        // Build the attribute
        var attributeName = stepKind.ToString(); // "Given", "When", or "Then"
        var attribute = $"[{attributeName}(\"{EscapePattern(pattern)}\")]";

        // Build parameter list
        var parameterList = new StringBuilder();
        for (int i = 0; i < parameters.Count; i++)
        {
            if (i > 0)
                parameterList.Append(", ");
            parameterList.Append($"{parameters[i].parameterType} {parameters[i].parameterName}");
        }

        // Add multiline parameter if needed
        if (hasMultilineParameter)
        {
            if (parameterList.Length > 0)
                parameterList.Append(", ");
            parameterList.Append("string multilineText");
        }

        // Add table parameter if needed
        if (hasTableParameter)
        {
            if (parameterList.Length > 0)
                parameterList.Append(", ");
            parameterList.Append("Reqnroll.Table table");
        }

        // Build method signature
        var returnType = isAsync ? "async Task" : "void";
        var methodSignature = $"public {returnType} {methodName}({parameterList})";

        // Build method body
        var body = isAsync
            ? "    await Task.CompletedTask;\n    throw new PendingStepException();"
            : "    throw new PendingStepException();";

        // Combine everything
        var code = new StringBuilder();
        code.AppendLine(attribute);
        code.AppendLine(methodSignature);
        code.AppendLine("{");
        code.AppendLine(body);
        code.Append("}");

        return code.ToString();
    }

    private string EscapePattern(string pattern)
    {
        // Escape backslashes and quotes for C# string literals
        return pattern.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}
