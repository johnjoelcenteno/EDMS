using System.ComponentModel;

namespace DPWH.EDMS.Shared.Enums;

public enum NavType
{
    [Description("Main Menu")]
    MainMenu,

    [Description("Current User Menu")]
    CurrentUserMenu,

    [Description("Settings")]
    Settings,
}
