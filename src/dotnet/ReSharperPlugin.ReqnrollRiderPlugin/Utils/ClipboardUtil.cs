using System.Windows.Forms;
using JetBrains.Application.Notifications;
using JetBrains.Application.Parts;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;

namespace ReSharperPlugin.ReqnrollRiderPlugin.Utils;

public interface IClipboardUtil
{
    void CopyToClipboard(string text);
    void ShowCopiedNotification();
}

[SolutionComponent(Instantiation.DemandAnyThreadUnsafe)]
public class ClipboardUtil : IClipboardUtil
{
    private readonly UserNotifications _userNotifications;
    private readonly Lifetime _lifetime;

    public ClipboardUtil(UserNotifications userNotifications, Lifetime lifetime)
    {
        _userNotifications = userNotifications;
        _lifetime = lifetime;
    }

    public void CopyToClipboard(string text)
    {
        System.Windows.Forms.Clipboard.SetText(text);
    }

    public void ShowCopiedNotification()
    {
        _userNotifications.CreateNotification(
            _lifetime,
            NotificationSeverity.INFO,
            "Step scaffolding copied",
            body: "The step definition code has been copied to clipboard and is ready to paste."
        );
    }
}
