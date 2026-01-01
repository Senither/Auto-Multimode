using Dalamud.Configuration;

namespace AutoMultimode;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 1;

    public bool Enabled { get; set; } = true;
    public int EnforcedAfkTime { get; set; } = 5;

    public void Save()
    {
        Service.PrintDebug("Saving configuration");
        AutoMultimode.PluginInterface.SavePluginConfig(this);
    }
}
