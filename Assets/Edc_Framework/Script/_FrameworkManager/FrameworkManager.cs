using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;


public class FrameworkManager : MonoBehaviour
{
    public static bool isInitFinish;
    
    [SerializeField]
    [LabelText("主摄像机")]
    private Camera mainCamera;

    [SerializeField]
    [LabelText("UI像机")]
    private Camera uiCamera;

    [Title("交互管理")]
    [SerializeField]
    private GameObject eventSystem;
    [SerializeField]
    private PhysicsRaycaster eaycaster3D;
    [SerializeField]
    private Physics2DRaycaster eaycaster2D;

    [Title("UI模块组件")]
    public UIManager uiController;
    public ViewManager viewController;
    public PersistentViewManager persistentViewController;
    public WindowManager windowController;
    public LoadingManager loadingController;
    public ScreenTransitionManager screenTransitionController;
    public NotificationManager notificationController;
    public CGManager cgController;

    [Title("")]
    public CoroutineRunner coroutineRunner;
    public AudioManager audioController;
    public UpdateManager updateController;
    
    [Title("")]
    [SerializeField, LabelText("框架运行时设置")]
    private FrameworkRuntimeSetting runtimeSetting;
    private static FrameworkManager instance;
    private static string initFinishLoadScene;
    public static Camera MainCamera {get {return instance.mainCamera;}}
    public static Camera UiCamera{get {return instance.uiCamera;}}
    public static FrameworkRuntimeSetting FrameworkSetting {get {return instance.runtimeSetting;}}
    public static LogLevel LogDisplay {get {return instance.runtimeSetting.logDisplay;}}
    public static bool IsSaveDisabled{get {return instance.runtimeSetting.isSaveDisabled;}}
    public static bool IsEditorUsingAssetBundle{get {return instance.runtimeSetting.isEditorUsingAssetBundle;}}
    private void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
        InitInfo();
        FrameworkInitFinish();
    }

    public static void SetInitFinishLoadScene(string initFinishLoadScene){
        FrameworkManager.initFinishLoadScene = initFinishLoadScene;
    }

    private void InitInfo(){
        Hub.Init();
        GameModule.Init();
        // Hub.EventCenter.AddListener(EventName.enterRestriction, EnterRestriction);
        // Hub.EventCenter.AddListener(EventName.exitRestriction, ExitRestriction);
    }

    private void FrameworkInitFinish()
    {
        isInitFinish = true;
        LogManager.Log("框架初始化完成");
        if(string.IsNullOrEmpty(initFinishLoadScene)){
            LogManager.Log("未定义框架初始化完成后进入的场景");
        }
        else {
            Hub.Scene.LoadScene(initFinishLoadScene);   
        }
    }

    private void EnterRestriction(){
        eventSystem.SetActive(false);
        eaycaster3D.enabled = false;
        eaycaster2D.enabled = false;
    }

    private void ExitRestriction(){
        eventSystem.SetActive(true);
        eaycaster3D.enabled = true;
        eaycaster2D.enabled = true;
    }
}
