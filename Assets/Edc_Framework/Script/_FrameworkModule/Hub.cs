using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Hub
{
    private static FrameworkManager framework;
    public static FrameworkManager Framework{get{return framework; }}

    private static ABManager ab;
    public static ABManager Ab{get{return ab; }}

    private static GameSceneManager scene;
    public static GameSceneManager Scene{get {return scene; }}

    private static ViewManager view;
    public static ViewManager View{get {return view; }}

    private static WindowManager window;
    public static WindowManager Window{get {return window; }}

    private static LoadPanel loadPanel;
    public static LoadPanel LoadPanel{get {return loadPanel; }}

    private static InputManager input;
    public static InputManager Input{get {return input; }}

    private static AudioManager audio;
    public static AudioManager Audio{get{return audio; }}

    private static AchievementManager achievement;
    public static AchievementManager Achievement{get{return achievement; }}

    private static RedDotTreeManager redDotTree;
    public static RedDotTreeManager RedDotTree{get{return redDotTree; }}

    private static LocalizationManager localization;
    public static LocalizationManager Localization{get{return localization; }}
    private static SOFileManager soFile;
    public static SOFileManager SoFile{get{return soFile;}}

    private static UpdateManager update;
    public static UpdateManager Update{get{return update;}}

    private static TimeRefreshFixedManager timeRefreshFixed;
    public static TimeRefreshFixedManager TimeRefreshFixed{get{return timeRefreshFixed;}}

    private static TimeRefreshSchedManager timeRefreshSched;
    public static TimeRefreshSchedManager TimeRefreshScheduled{get{return timeRefreshSched;}}


    public static void Init(FrameworkManager framework){
        Hub.framework = framework;
        BasePool.InitPoolSetting();

        ab = new ABManager();            
        scene = new GameSceneManager();

        view = new ViewManager();
        window = new WindowManager();      
        loadPanel = new LoadPanel();
        
        input = new InputManager();

        audio = new AudioManager();
        
        achievement = new AchievementManager();

        redDotTree = new RedDotTreeManager();
        
        localization = new LocalizationManager();
        
        soFile = new SOFileManager();

        update = Hub.framework.GetComponent<UpdateManager>();
        
        timeRefreshFixed = new TimeRefreshFixedManager();

        timeRefreshSched = new TimeRefreshSchedManager();
    }
}
