public class GameModule
{
    private static CameraModule camera;
    public static CameraModule Camera{get{return camera; }}

    public static void Init(){
        camera = new CameraModule();
    }
}
