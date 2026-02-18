using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.OptionPages.CodeEditing;

namespace ReSharperPlugin.ReqnrollRiderPlugin.Settings;

[SettingsKey(typeof(CodeEditingPage), "Reqnroll Plugin Settings")]
public class ReqnrollPluginSettings
{
    [SettingsEntry(false, "Generate async step methods (Task) by default")]
    public bool GenerateAsyncSteps;
}
