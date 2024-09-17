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


    private static TimeRefreshFixedManager timeRefreshFixed;
    public static TimeRefreshFixedManager TimeRefreshFixed{get{return timeRefreshFixed;}}

    private static TimeRefreshSchedManager timeRefreshSched;
    public static TimeRefreshSchedManager TimeRefreshScheduled{get{return timeRefreshSched;}}

    private static UpdateController update;
    public static UpdateController Update{get{return update;}}

    private static UIController ui;
    public static UIController UI{get{return ui;}}

    public static CGController cg;
    public static CGController CG{get {return cg; }}

    private static ViewController view;
    public static ViewController View{get {return view; }}

    private static PersistentViewController persistentView;
    public static PersistentViewController PersistentView{get {return persistentView; }}

    private static WindowController window;
    public static WindowController Window{get {return window; }}

    private static ScreenTransitionController screenTransition;
    public static ScreenTransitionController ScreenTransition{get {return screenTransition; }}

    

    public static void Init(FrameworkManager framework){
        Hub.framework = framework;
        BasePool.InitPoolSetting();
        
        update = framework.GetComponent<UpdateController>();
        cg = framework.GetComponentInChildren<CGController>();
        view = framework.GetComponentInChildren<ViewController>();
        persistentView = framework.GetComponentInChildren<PersistentViewController>();
        window = framework.GetComponentInChildren<WindowController>();
        screenTransition = framework.GetComponentInChildren<ScreenTransitionController>();
        ui = framework.GetComponentInChildren<UIController>();
        cg.Init();
        view.Init();
        persistentView.Init();
        window.Init();
        window.Init();
        ui.SetAdaptation();

        ab = new ABManager();            
        scene = new GameSceneManager();
        
        input = new InputManager();

        audio = new AudioManager();
        
        achievement = new AchievementManager();

        redDotTree = new RedDotTreeManager();
        
        localization = new LocalizationManager();
        
        soFile = new SOFileManager();
        
        timeRefreshFixed = new TimeRefreshFixedManager();

        timeRefreshSched = new TimeRefreshSchedManager();
    }
}
