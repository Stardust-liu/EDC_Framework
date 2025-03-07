using UnityEngine;

public class Hub : IOCContainer<Hub>
{    
    public static CoroutineRunner Coroutine{get; private set;}
    public static ResourcesModule Resources{get; private set;}
    public static GameSceneManager Scene{get; private set;}
    public static InputManager Input{get; private set;}
    public static AudioController Audio{get; private set;}
    public static UpdateController Update{get; private set;}
    public static AchievementManager Achievement{get; private set;}
    public static RedDotTreeManager RedDotTree{get; private set;}
    public static LocalizationManager Localization{get; private set;}
    public static TimeRefreshFixedManager TimeRefreshFixed{get; private set;}
    public static TimeRefreshSchedManager TimeRefreshScheduled{get; private set;}

#region UI相关
    public static UIController UI{get; private set;}
    public static ViewController View{get; private set;}
    public static PersistentViewController PersistentView{get; private set;}
    public static WindowController Window{get; private set;}
    public static LoadingController Loading{get; private set;}
    public static ScreenTransitionController ScreenTransition{get; private set;}
    public static NotificationController Notification{get; private set;}
    public static CGController CG{get; private set;}
#endregion
    
    protected override void InitContainer(){
        var framework = GameObject.Find("MainObject").GetComponent<FrameworkManager>();
        Resources = ((IContainer)this).Register<ResourcesModule>();
        Resources = ((IContainer)this).Register<ResourcesModule>();

        Coroutine = ((IContainer)this).Register(framework.coroutineRunner);
        Audio = ((IContainer)this).Register(framework.audioController);
        Update = ((IContainer)this).Register(framework.updateController);

        UI = ((IContainer)this).Register(framework.uiController);
        View = ((IContainer)this).Register(framework.viewController);
        PersistentView = ((IContainer)this).Register(framework.persistentViewController);
        Window = ((IContainer)this).Register(framework.windowController);
        Loading = ((IContainer)this).Register(framework.loadingController);
        ScreenTransition =  ((IContainer)this).Register(framework.screenTransitionController);
        Notification = ((IContainer)this).Register(framework.notificationController);
        CG = ((IContainer)this).Register(framework.cgController);

        Scene = ((IContainer)this).Register<GameSceneManager>();
        Input = ((IContainer)this).Register<InputManager>();
        Achievement = ((IContainer)this).Register<AchievementManager>();
        RedDotTree = ((IContainer)this).Register<RedDotTreeManager>();
        Localization = ((IContainer)this).Register<LocalizationManager>();
        TimeRefreshFixed = ((IContainer)this).Register<TimeRefreshFixedManager>();
    }
}
