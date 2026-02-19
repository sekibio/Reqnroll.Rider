using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using ReSharperPlugin.ReqnrollRiderPlugin.Extensions;
using ReSharperPlugin.ReqnrollRiderPlugin.Psi;
using ReSharperPlugin.ReqnrollRiderPlugin.References;
using ReSharperPlugin.ReqnrollRiderPlugin.Settings;
using ReSharperPlugin.ReqnrollRiderPlugin.Utils;
using ReSharperPlugin.ReqnrollRiderPlugin.Utils.Steps;

namespace ReSharperPlugin.ReqnrollRiderPlugin.QuickFixes.CreateMissingStep;

public class CopyStepScaffoldingToClipboardAction : IBulbAction
{
    private readonly ReqnrollStepDeclarationReference _reference;
    private readonly IStepScaffoldingGenerator _scaffoldingGenerator;
    private readonly IClipboardUtil _clipboardUtil;
    private readonly ISettingsStore _settingsStore;

    public CopyStepScaffoldingToClipboardAction(
        ReqnrollStepDeclarationReference reference,
        IStepScaffoldingGenerator scaffoldingGenerator,
        IClipboardUtil clipboardUtil,
        ISettingsStore settingsStore)
    {
        _reference = reference;
        _scaffoldingGenerator = scaffoldingGenerator;
        _clipboardUtil = clipboardUtil;
        _settingsStore = settingsStore;
    }

    public string Text => "Copy step scaffolding to clipboard";

    public void Execute(ISolution solution, ITextControl textControl)
    {
        // Read async preference from settings
        var contextBoundSettingsStore = _settingsStore.BindToContextTransient(ContextRange.Smart(_reference.GetTreeNode().ToDataContext()));
        var settings = contextBoundSettingsStore.GetKey<ReqnrollPluginSettings>(SettingsOptimization.OptimizeDefault);
        var isAsync = settings.GenerateAsyncSteps;

        // Get step information
        var stepKind = _reference.GetStepKind();
        var stepText = _reference.GetStepText();
        var culture = _reference.GetGherkinFileCulture();

        // Check for multiline and table parameters
        var gherkinStep = _reference.GetElement();
        var hasMultilineParameter = gherkinStep.Children<GherkinPystring>().Any();
        var hasTableParameter = gherkinStep.Children<GherkinTable>().Any();

        // Generate scaffolding code
        var code = _scaffoldingGenerator.GenerateScaffoldingCode(
            stepKind,
            stepText,
            culture,
            isAsync,
            hasMultilineParameter,
            hasTableParameter
        );

        // Copy to clipboard
        _clipboardUtil.CopyToClipboard(code);

        // Show notification
        _clipboardUtil.ShowCopiedNotification();
    }
}
