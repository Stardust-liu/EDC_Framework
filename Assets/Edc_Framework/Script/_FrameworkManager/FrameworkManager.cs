using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

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
    public InteractionManager interactionController;


    [Title("UI模块组件")]
    public UIManager uiController;
    public ViewManager viewController;
    public PersistentViewManager persistentViewController;
    public WindowManager windowController;
    public LoadingManager loadingController;
    public ScreenTransitionManager screenTransitionController;
    public NotificationManager notificationController;
    [LabelText("CG管理")]
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
    public static Camera MainCamera { get { return instance.mainCamera; } }
    public static Camera UiCamera { get { return instance.uiCamera; } }
    public static FrameworkRuntimeSetting FrameworkSetting { get { return instance.runtimeSetting; } }
    public static LogLevel LogDisplay { get { return instance.runtimeSetting.logDisplay; } }
    public static bool IsSaveDisabled { get { return instance.runtimeSetting.isSaveDisabled; } }
    public static bool IsShowLogo { get { return instance.runtimeSetting.isShowLogo; } }
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        StartFrameworkFlow().Forget();
    }

    private void OnApplicationQuit()
    {
        HotUpdateLoader.Quit();
        Hub.Quit();
    }

    private async UniTaskVoid StartFrameworkFlow()
    {
        try 
        {
            await InitInfo();
        }
        catch (Exception e)
        {
            LogManager.LogError($"框架启动崩溃: {e}");
        }
    }

    private async UniTask InitInfo()
    {
        await Hub.Init(instance);
        Hub.ReadyRegisteredModules();
        FrameworkInitFinish();
        await HotUpdateLoader.Load();
        await HotUpdateLoader.Init(CreateGameStartInfo());
        await HotUpdateLoader.ReadyRegisteredModules();
        await HotUpdateLoader.EnterGame();
    }

    private void FrameworkInitFinish()
    {
        isInitFinish = true;
        LogManager.Log("框架初始化完成");
    }

    private GameStartInfo CreateGameStartInfo()
    {
        return GameStartInfo.CreateGameStartInfo(initFinishLoadScene, IsShowLogo);
    }

    /// <summary>
    /// 设置加载完成后跳转的场景
    /// </summary>
    public static void SetInitFinishLoadScene(string sceneName)
    {
        initFinishLoadScene = sceneName;
    }
}
