using System.Collections.Generic;

public sealed class SteamProfile : BuildProfile
{
    private static readonly BuildModuleFolder[] Modules =
    {
        new BuildModuleFolder("Steam", "Steam"),
    };

    private static readonly string[] Defines = { "EDC_STEAM" };

    public override string ProfileId => "Steam";
    public override string DisplayName => "Steam 正式发布";
    public override int SortOrder => 20;
    public override IReadOnlyList<BuildModuleFolder> ModuleFolders => Modules;
    public override IReadOnlyList<string> DefineSymbols => Defines;
}
