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

    public string GetEnforcedAfkTimerString()
    {
        switch (EnforcedAfkTimer)
        {
            case 1:
                return "5m";
            case 2:
                return "10m";
            case 3:
                return "30m";
            case 4:
                return "60m";
            default:
                return "OFF";
        }
    }
}
