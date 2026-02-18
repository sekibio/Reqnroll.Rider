using JetBrains.Application.Settings;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.Feature.Services.OptionPages.CodeEditing;
using JetBrains.ReSharper.Resources.Resources.Icons;

namespace ReSharperPlugin.ReqnrollRiderPlugin.Settings;

[OptionsPage(
    PageId,
    "Reqnroll",
    typeof(PsiFeaturesUnsortedOptionsThemedIcons.Gherkin),
    ParentId = CodeEditingPage.PID,
    Sequence = 100
)]
public class ReqnrollPluginSettingsPage : OptionsPageBase
{
    public const string PageId = "ReqnrollPluginSettingsPage";

    public ReqnrollPluginSettingsPage(
        Lifetime lifetime,
        OptionsPageContext optionsPageContext,
        OptionsSettingsSmartContext optionsSettingsSmartContext)
        : base(lifetime, optionsPageContext, optionsSettingsSmartContext)
    {
        AddBoolOption(
            (ReqnrollPluginSettings s) => s.GenerateAsyncSteps,
            "Generate async step methods (Task) by default",
            "When enabled, generated step definitions will use 'async Task' instead of 'void'");
    }
}
