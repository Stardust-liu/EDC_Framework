using System.Collections.Generic;

public sealed class DevProfile : BuildProfile
{
    private static readonly string[] Defines = { "EDC_DEV" };

    public override string ProfileId => "Dev";
    public override string DisplayName => "Dev 内部测试";
    public override int SortOrder => 0;
    public override IReadOnlyList<string> DefineSymbols => Defines;
    public override bool DevelopmentBuild => true;
    public override string VersionSuffix => "dev";
}
