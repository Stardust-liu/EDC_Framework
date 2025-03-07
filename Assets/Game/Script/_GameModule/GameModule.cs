public class GameModule : IOCContainer<GameModule>
{
    public static CameraModule Camera{get; private set;}

    protected override void InitContainer()
    {
        Camera = ((IContainer)Instance).Register<CameraModule>();
    }
}
