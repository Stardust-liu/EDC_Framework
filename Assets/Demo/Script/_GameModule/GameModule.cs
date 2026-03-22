public class GameModule : IOCContainer<GameModule>
{
    public static CameraModule Camera{get{return camera;}}
    public static LevelModel Level{get{return level;}}
    public static CameraModule camera;
    public static LevelModel level;

    protected override void InitContainer()
    {
        ((IContainer)Instance).Register(out camera);
        ((IContainer)Instance).Register(out level);
    }
}
