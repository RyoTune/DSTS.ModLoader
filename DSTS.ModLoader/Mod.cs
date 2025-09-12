#if DEBUG
using System.Diagnostics;
#endif
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using DSTS.ModLoader.Template;
using DSTS.ModLoader.Configuration;
using Reloaded.Mod.Interfaces.Internal;

namespace DSTS.ModLoader;

public class Mod : ModBase
{
    private readonly IModLoader _modLoader;
    private readonly IReloadedHooks? _hooks;
    private readonly ILogger _log;
    private readonly IMod _owner;

    public static Config Config = null!;
    private readonly IModConfig _modConfig;
    private readonly DstsModRegistry _registry;
    private readonly DstsModLoader _dstsLoader;

    public Mod(ModContext context)
    {
        _modLoader = context.ModLoader;
        _hooks = context.Hooks;
        _log = context.Logger;
        _owner = context.Owner;
        Config = context.Configuration;
        _modConfig = context.ModConfig;
#if DEBUG
        Debugger.Launch();
#endif
        Project.Initialize(_modConfig, _modLoader, _log, true);
        Log.LogLevel = Config.LogLevel;

        _registry = new();
        _dstsLoader = new(_registry);
        
        _modLoader.ModLoaded += ModLoaded;
    }

    private void ModLoaded(IModV1 mod, IModConfigV1 modConfig)
    {
        if (!Project.IsModDependent(modConfig)) return;

        var modDir = _modLoader.GetDirectoryForModId(modConfig.ModId);
        var digiDir = Path.Join(modDir, "dsts-loader");
        if (!Directory.Exists(digiDir)) return;
        
        var numFiles = _registry.AddFolder(digiDir);
        Log.Information($"Registered Mod: {modConfig.ModName} || Total Files: {numFiles}");
    }

    #region Standard Overrides

    public override void ConfigurationUpdated(Config configuration)
    {
        // Apply settings from configuration.
        // ... your code here.
        Config = configuration;
        _log.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        Log.LogLevel = Config.LogLevel;
    }

    #endregion

    #region For Exports, Serialization etc.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Mod()
    {
    }
#pragma warning restore CS8618

    #endregion
}