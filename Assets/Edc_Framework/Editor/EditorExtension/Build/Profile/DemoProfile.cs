using System.Collections.Generic;

public sealed class DemoProfile : BuildProfile
{
    private static readonly string[] Defines = { "EDC_DEMO" };

    public override string ProfileId => "Demo";
    public override string DisplayName => "Demo 外部试玩";
    public override int SortOrder => 10;
    public override IReadOnlyList<string> DefineSymbols => Defines;
    public override string VersionSuffix => "demo";
}
