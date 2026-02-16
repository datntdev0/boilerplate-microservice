using Microsoft.AspNetCore.Components;

namespace datntdev.Microservice.App.Identity.Components.Shared;

public partial class AlertDialog
{
    [Parameter] public AlertDialogType Type { get; set; } = AlertDialogType.Info;

    [Parameter] public string Title { get; set; } = string.Empty;

    [Parameter] public string Text { get; set; } = string.Empty;

    [Parameter] public string ConfirmButtonText { get; set; } = string.Empty;

    [Parameter] public AlertDialogButtonVariant ConfirmButtonVariant { get; set; }

    [Parameter] public string CancelButtonText { get; set; } = string.Empty;

    [Parameter] public AlertDialogButtonVariant CancelButtonVariant { get; set; }

    [Parameter] public bool IsVisible { get; set; }
}

public enum AlertDialogType
{
    Info,
    Danger,
    Warning,
    Success,
}

public enum AlertDialogButtonVariant
{
    Primary,
    Secondary,
    Success,
    Danger,
    Warning,
    Info,
}