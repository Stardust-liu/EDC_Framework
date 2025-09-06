public class GameModule : IOCContainer<GameModule>
{
    public static CameraModule Camera{get; private set;}
    public static LevelModel Level{get; private set;}

    protected override void InitContainer()
    {
        Camera = ((IContainer)Instance).Register<CameraModule>();
        Level = ((IContainer)Instance).Register<LevelModel>();
    }
}
