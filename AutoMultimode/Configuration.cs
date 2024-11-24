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
}
