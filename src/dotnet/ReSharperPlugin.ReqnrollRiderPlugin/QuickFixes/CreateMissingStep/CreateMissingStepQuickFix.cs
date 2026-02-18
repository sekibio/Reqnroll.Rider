using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.Application.UI.Icons.CommonThemedIcons;
using JetBrains.ReSharper.Feature.Services.Intentions;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.Util;
using ReSharperPlugin.ReqnrollRiderPlugin.Caching.StepsDefinitions;
using ReSharperPlugin.ReqnrollRiderPlugin.Daemon.Errors;
using ReSharperPlugin.ReqnrollRiderPlugin.Utils;
using ReSharperPlugin.ReqnrollRiderPlugin.Utils.Steps;

namespace ReSharperPlugin.ReqnrollRiderPlugin.QuickFixes.CreateMissingStep;

[QuickFix]
public class CreateMissingStepQuickFix : IQuickFix
{
    private readonly StepNotResolvedError _error;
    private readonly IgnoredStepNotResolvedInfo _info;

    public CreateMissingStepQuickFix(StepNotResolvedError error)
    {
        _error = error;
    }

    public CreateMissingStepQuickFix(IgnoredStepNotResolvedInfo info)
    {
        _info = info;
    }

    public IEnumerable<IntentionAction> CreateBulbItems()
    {
        var gherkinStep = _error?.GherkinStep ?? _info?.GherkinStep;
        if (gherkinStep == null)
            return [];
        var psiServices = gherkinStep.GetPsiServices();
        var stepReference = gherkinStep.GetStepReference();

        var actions = new List<IntentionAction>
        {
            // Original action: Create step definition in a file
            new IntentionAction(new CreateReqnrollStepFromUsageAction(
                stepReference,
                psiServices.GetComponent<IMenuModalUtil>(),
                psiServices.GetComponent<ICreateStepClassDialogUtil>(),
                psiServices.GetComponent<ICreateStepPartialClassFile>(),
                psiServices.GetComponent<ReqnrollStepsDefinitionsCache>(),
                psiServices.GetComponent<ICreateReqnrollStepUtil>()
            ), BulbThemedIcons.YellowBulb.Id, IntentionsAnchors.QuickFixesAnchor),

            // New action: Copy scaffolding to clipboard
            new IntentionAction(new CopyStepScaffoldingToClipboardAction(
                stepReference,
                psiServices.GetComponent<IStepScaffoldingGenerator>(),
                psiServices.GetComponent<IClipboardUtil>(),
                psiServices.GetComponent<ISettingsStore>()
            ), CommonThemedIcons.Copy.Id, IntentionsAnchors.QuickFixesAnchor)
        };

        return actions;
    }

    public bool IsAvailable(IUserDataHolder cache)
    {
        return true;
    }
}