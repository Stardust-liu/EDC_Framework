using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using ArchiveData;

public class FrameworkManager : MonoBehaviour
{
    public static bool isInitFinish;

    [SerializeField]
    [LabelText("显示打印信息的种类")]
    private LogLevel logDisplay;
    
    [SerializeField]
    [LabelText("主摄像机")]
    private Camera mainCamera;

    [SerializeField]
    [LabelText("UI像机")]
    private Camera uiCamera;

    [SerializeField]
    [LabelText("是否禁用数据保存功能")]
    private bool isSaveDisabled;

    [Title("交互管理")]
    [SerializeField]
    private GameObject eventSystem;
    [SerializeField]
    private PhysicsRaycaster eaycaster3D;
    [SerializeField]
    private Physics2DRaycaster eaycaster2D;

    [Title("UI模块组件")]
    public UIController uiController;
    public ViewController viewController;
    public PersistentViewController persistentViewController;
    public WindowController windowController;
    public LoadingController loadingController;
    public ScreenTransitionController screenTransitionController;
    public NotificationController notificationController;
    public CGController cgController;

    [Title("")]
    public CoroutineRunner coroutineRunner;
    public AudioController audioController;
    public UpdateController updateController;

    private static FrameworkManager instance;
    private static string initFinishLoadScene;
    public static LogLevel LogDisplay {get {return instance.logDisplay;}}
    public static Camera MainCamera {get {return instance.mainCamera;}}
    public static Camera UiCamera{get {return instance.uiCamera;}}
    public static bool IsSaveDisabled{get {return instance.isSaveDisabled;}}
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
        GameArchive.Init();
        Hub.Init();
        GameModule.Init();
        // Hub.EventCenter.AddListener(EventName.enterRestriction, EnterRestriction);
        // Hub.EventCenter.AddListener(EventName.exitRestriction, ExitRestriction);
    }

    private void FrameworkInitFinish(){
        isInitFinish = true;
        Hub.Scene.LoadScene(initFinishLoadScene);
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
