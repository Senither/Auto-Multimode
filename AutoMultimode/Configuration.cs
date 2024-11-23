using System;
using Dalamud.Configuration;

namespace AutoMultimode;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;
    public uint EnforcedAfkTimer { get; set; } = 1;

    public void Save()
    {
        Service.PrintDebug("Saving configuration");
        AutoMultimode.PluginInterface.SavePluginConfig(this);
    }

    public int GetEnforcedAfkTimerInSeconds()
    {
        switch (EnforcedAfkTimer)
        {
            case 1:
                return 60 * 5;
            case 2:
                return 60 * 10;
            case 3:
                return 60 * 30;
            case 4:
                return 60 * 60;

            default:
                return 60;
        }
    }
}
