using AutoMultimode.IPC;
using ECommons;
using ECommons.Logging;

namespace AutoMultimode;

public class CommandHandler
{
    private AutoMultimode Plugin;

    public CommandHandler(AutoMultimode plugin)
    {
        Plugin = plugin;
    }

    public void HandleCommand(string[] args)
    {
        if (args.Length == 0)
        {
            Plugin.ToggleMainUI();
            return;
        }

        if (args[0].EqualsIgnoreCaseAny(new List<string> { "config", "cfg", "c" }))
        {
            Plugin.ToggleConfigUI();
            return;
        }

        if (args[0].EqualsIgnoreCaseAny(new List<string> { "status", "sta", "s" }))
        {
            string status = AutoRetainerIPC.IsEnabled ? "Enabled" : "Disabled";

            DuoLog.Information("AutoRetainer status:                 " + status);
            DuoLog.Information("AutoMultiMode AFK timer:   " + Plugin.Configuration.GetEnforcedAfkTimerString());
            return;
        }

        if (args[0].EqualsIgnoreCaseAny(new List<string> { "debug", "d" }))
        {
            AutoMultimode.PluginInterface.OpenDeveloperMenu();
            return;
        }

        DuoLog.Warning("Invalid argument, example usage /automultimode <option>");
        DuoLog.Warning("Options:");
        DuoLog.Warning(" - config / cfg / c");
        DuoLog.Warning(" - status / sta / s");
        DuoLog.Warning(" - debug / d");
    }
}
