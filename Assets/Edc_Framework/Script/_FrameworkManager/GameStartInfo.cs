public enum GameStartEnvironment
{
    Editor,
    Player
}

public readonly struct GameStartInfo
{
    public GameStartEnvironment Environment { get; }
    public string TargetSceneName { get; }
    public bool IsShowLogo { get;}
    public bool HasTargetScene => !string.IsNullOrEmpty(TargetSceneName);

    public static GameStartInfo CreateGameStartInfo(string targetSceneName, bool isShowLogo)
    {
#if UNITY_EDITOR
    var environment = GameStartEnvironment.Editor;
#else
    var environment = GameStartEnvironment.Player;
#endif
        if (string.IsNullOrEmpty(targetSceneName))
        {
            return new(environment, null, isShowLogo);
        }
        return new(environment, targetSceneName, isShowLogo);
    }

    private GameStartInfo(GameStartEnvironment environment, string targetSceneName, bool isShowLogo)
    {
        Environment = environment;
        TargetSceneName = targetSceneName;
        IsShowLogo = isShowLogo;
    }
}
