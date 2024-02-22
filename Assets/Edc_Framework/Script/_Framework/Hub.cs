using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Hub
{
    private static ABManager ab;
    public static ABManager Ab{get{return ab; }}

    private static EventCenter eventCenter;
    public static EventCenter EventCenter{get {return eventCenter; }}

    private static GameArchiveManager data;
    private static GameArchiveManager Data{get {return data; }}

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

    private static LanguageManager language;
    public static LanguageManager Language{get{return language; }}

    public static void Init(FrameworkManager framework){
        ab = new ABManager();

        eventCenter = new EventCenter();
        
        data = new GameArchiveManager();
    
        scene = new GameSceneManager();

        view = new ViewManager(framework.viewLayer, framework.persistentViewLayer, framework.view);
        window = new WindowManager(framework.windowLayer, framework.windowDarkBG, framework.window);      
        loadPanel = new LoadPanel(framework.loadPanel);
        
        input = new InputManager();

        audio = new AudioManager(framework.bGM1, framework.bGM2);
        audio.Init(framework.sFXParent, framework.sFXParent);

        language = new LanguageManager();

        BasePool.Init(framework.objectPoolSetting);
        LoadAssetFile.Init();
    }
}
