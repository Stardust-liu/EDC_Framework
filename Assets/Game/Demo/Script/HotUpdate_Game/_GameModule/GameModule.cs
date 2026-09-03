using Cysharp.Threading.Tasks;

public class GameModule : IOCContainer<GameModule>
{
    public static GameConfigManager GameConfig{get{return gameConfig;}}
    public static CameraModule Camera{get{return camera;}}
    public static LevelModel Level{get{return level;}}
    private static GameConfigManager gameConfig;
    private static CameraModule camera;
    private static LevelModel level;

    protected override async UniTask InitContainer()
    {
        await ((IContainer)Instance).Register(out gameConfig).LoadLabel();
        ((IContainer)Instance).Register(out camera);
        ((IContainer)Instance).Register(out level);
    }
}
