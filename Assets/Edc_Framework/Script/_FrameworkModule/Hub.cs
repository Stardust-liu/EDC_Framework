using Cysharp.Threading.Tasks;
using UnityEngine;

public class Hub : IOCContainer<Hub>
{    
    public static ResourcesModule Resources{get{return resources;}}
    public static FrameworkConfigManager FrameworkConfig{get{return frameworkConfig;}}
    public static CoroutineRunner Coroutine{get{return coroutine;}}
    public static GameSceneManager Scene{get{return scene;}}
    public static InputManager Input{get{return input;}}
    public static AudioManager Audio{get{return audio;}}
    public static UpdateManager Update{get{return update;}}
    public static AchievementManager Achievement{get{return achievement;}}
    public static RedDotTreeManager RedDotTree{get{return redDotTree;}}
    public static LocalizationManager Localization{get{return localization;}}
    public static InteractionManager Interaction{get{return interaction;}}
    
#region UI相关
    public static UIManager UI {get{return ui;}}
    public static ViewManager View{get{return view;}}
    public static PersistentViewManager PersistentView{get{return persistentView;}}
    public static WindowManager Window{get{return window;}}
    public static LoadingManager Loading{get{return loading;}}
    public static ScreenTransitionManager ScreenTransition{get{return screenTransition;}}
    public static NotificationManager Notification{get{return notification;}}
    public static CGManager CG{get{return cg;}}
#endregion

    private static ResourcesModule resources;
    private static FrameworkConfigManager frameworkConfig;
    private static CoroutineRunner coroutine;
    private static GameSceneManager scene;
    private static InputManager input;
    private static AudioManager audio;
    private static UpdateManager update;
    private static AchievementManager achievement;
    private static RedDotTreeManager redDotTree;
    private static LocalizationManager localization;
    private static InteractionManager interaction;

    private static UIManager ui;
    private static ViewManager view;
    private static PersistentViewManager persistentView;
    private static WindowManager window;
    private static LoadingManager loading;
    private static ScreenTransitionManager screenTransition;
    private static NotificationManager notification;
    private static CGManager cg;

    protected override async UniTask InitContainer(FrameworkManager framework)
    {
        ((IContainer)this).Register(out resources);
        await ((IContainer)this).Register(out frameworkConfig).LoadLabel();

        ((IContainer)this).Register(framework.coroutineRunner, out coroutine);
        ((IContainer)this).Register(framework.audioController, out audio);
        ((IContainer)this).Register(framework.updateController, out update);

        ((IContainer)this).Register(framework.interactionController, out interaction);
        ((IContainer)this).Register(framework.uiController, out ui);
        ((IContainer)this).Register(framework.viewController, out view);
        ((IContainer)this).Register(framework.persistentViewController, out persistentView);
        ((IContainer)this).Register(framework.windowController, out window);
        ((IContainer)this).Register(framework.loadingController, out loading);
        ((IContainer)this).Register(framework.screenTransitionController, out screenTransition);
        ((IContainer)this).Register(framework.notificationController, out notification);
        ((IContainer)this).Register(framework.cgController, out cg);

        ((IContainer)this).Register(out scene);
        ((IContainer)this).Register(out input);
        ((IContainer)this).Register(out achievement);
        ((IContainer)this).Register(out redDotTree);
        ((IContainer)this).Register(out localization);
    }
}
