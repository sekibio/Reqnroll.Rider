using System.Globalization;
using ReSharperPlugin.ReqnrollRiderPlugin.Psi;

namespace ReSharperPlugin.ReqnrollRiderPlugin.Utils.Steps;

public interface IStepScaffoldingGenerator
{
    string GenerateScaffoldingCode(
        GherkinStepKind stepKind,
        string stepText,
        CultureInfo culture,
        bool isAsync,
        bool hasMultilineParameter = false,
        bool hasTableParameter = false
    );
}
