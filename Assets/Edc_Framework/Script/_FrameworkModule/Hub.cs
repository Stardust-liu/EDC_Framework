public static class Hub
{    
    private static CoroutineRunner coroutine;
    public static CoroutineRunner Coroutine{get{return coroutine; }}

    private static ResourcesModule resources;
    public static ResourcesModule Resources{get{return resources; }}

    private static GameSceneManager scene;
    public static GameSceneManager Scene{get {return scene; }}

    private static InputManager input;
    public static InputManager Input{get {return input; }}

    private static AudioController audio;
    public static AudioController Audio{get{return audio; }}
    
    private static UpdateController update;
    public static UpdateController Update{get{return update;}}

    private static AchievementManager achievement;
    public static AchievementManager Achievement{get{return achievement; }}

    private static RedDotTreeManager redDotTree;
    public static RedDotTreeManager RedDotTree{get{return redDotTree; }}

    private static LocalizationManager localization;
    public static LocalizationManager Localization{get{return localization; }}

    private static TimeRefreshFixedManager timeRefreshFixed;
    public static TimeRefreshFixedManager TimeRefreshFixed{get{return timeRefreshFixed;}}

    private static TimeRefreshSchedManager timeRefreshSched;
    public static TimeRefreshSchedManager TimeRefreshScheduled{get{return timeRefreshSched;}}

#region UI相关
    private static UIController ui;
    public static UIController UI{get{return ui;}}

    private static ViewController view;
    public static ViewController View{get {return view; }}

    private static PersistentViewController persistentView;
    public static PersistentViewController PersistentView{get {return persistentView; }}

    private static WindowController window;
    public static WindowController Window{get {return window; }}

    private static LoadingController loading;
    public static LoadingController Loading{get {return loading; }}

    private static ScreenTransitionController screenTransition;
    public static ScreenTransitionController ScreenTransition{get {return screenTransition; }}

    private static NotificationController notification;
    public static NotificationController Notification{get {return notification; }}

    public static CGController cg;
    public static CGController CG{get {return cg; }}
#endregion

    public static void Init(FrameworkManager framework){
        resources = new ResourcesModule();    

        coroutine = framework.coroutineRunner;
        audio = framework.audioController;
        update = framework.updateController;

        ui = framework.uiController;
        view = framework.viewController;
        persistentView = framework.persistentViewController;
        window = framework.windowController;
        loading = framework.loadingController;
        screenTransition = framework.screenTransitionController;
        notification = framework.notificationController;
        cg = framework.cgController;

        audio.Init();
        ui.Init();
        view.Init();
        persistentView.Init();
        window.Init();
        cg.Init();

        scene = new GameSceneManager();
        
        input = new InputManager();
        
        achievement = new AchievementManager();

        redDotTree = new RedDotTreeManager();
        
        localization = new LocalizationManager();
                
        timeRefreshFixed = new TimeRefreshFixedManager();

        //timeRefreshSched = new TimeRefreshSchedManager();
    }
}
