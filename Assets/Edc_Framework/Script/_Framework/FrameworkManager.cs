using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FrameworkManager : MonoBehaviour
{
    public static bool isInitFinish;

    [LabelWidth(225)]
    [LabelText("是否标记勾选了RaycastTarget的UI")]
    public bool isMarkRaycastTargetUI;

    [LabelText("显示打印信息的种类")]
    public LogLevel logDisplay;
    
    [SerializeField]
    [LabelText("主摄像机")]
    protected Camera mainCamera;

    [SerializeField]
    [LabelText("UI像机")]
    protected Camera uiCamera;

    [Title("交互管理")]
    [SerializeField]
    protected GameObject eventSystem;
    [SerializeField]
    protected PhysicsRaycaster eaycaster3D;
    [SerializeField]
    protected Physics2DRaycaster eaycaster2D;

    [Title("音频管理")]
    public AudioSource bGM1;
    public AudioSource bGM2;
    public Transform sFXParent;
    public Transform vOParent;
    [SerializeField]
    protected SoundBgSetting soundBgSetting;
    [SerializeField]
    protected SoundEffectSetting soundEffectSetting;
    [SerializeField]
    protected SoundDialogueSetting soundDialogueSetting;

    [Title("按键输入管理")]
    [SerializeField]
    protected InputSetting inputSetting;

    [Title("对象池管理")]
    [SerializeField]
    public ObjectPoolSetting objectPoolSetting;

    [Title("UI管理")]
    [LabelText("视图界面UI管理")]
    public ViewPrefabSetting view;

    [LabelText("窗口UI管理")]
    public WindowPrefabSetting window;
    public CanvasScaler canvas;
    public RectTransform viewLayer;
    public RectTransform persistentViewLayer;
    public RectTransform windowLayer;
    public Image windowDarkBG;
    public Image loadPanel;
    public static FrameworkManager instance;
    private static string initFinishLoadScene;
    public static LogLevel LogDisplay {get {return instance.logDisplay;}}
    public static Camera MainCamera {get {return instance.mainCamera;}}
    public static Camera UiCamera{get {return instance.uiCamera;}}
    public static SoundBgSetting SoundBgSetting{get {return instance.soundBgSetting;}}
    public static SoundEffectSetting SoundEffectSetting{get {return instance.soundEffectSetting;}}
    public static SoundDialogueSetting SoundDialogueSetting{get {return instance.soundDialogueSetting;}}
    public static InputSetting InputSetting{get {return instance.inputSetting;}}
    public static WindowPrefabSetting Window{get {return instance.window;}}

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
        InitInfo();
        FrameworkInitFinish();
    }

    private void InitInfo(){
        Hub.Init(this);
        Hub.EventCenter.AddListener(EventName.enterRestriction, EnterRestriction);
        Hub.EventCenter.AddListener(EventName.exitRestriction, ExitRestriction);
    }

    private void FrameworkInitFinish(){
        isInitFinish = true;
        if(initFinishLoadScene == null){
            initFinishLoadScene = "MenuScene";
        }
        else{
            Hub.View.ChangeSceneDefaultView(initFinishLoadScene);
        }
        Hub.Scene.LoadScene(initFinishLoadScene);
    }

    public static void SetInitFinishLoadScene(string initFinishLoadScene){
        FrameworkManager.initFinishLoadScene = initFinishLoadScene;
    }

    private void OnDrawGizmos(){
        if (isMarkRaycastTargetUI)
        {
            Vector3[] boxSize = new Vector3[4];
            foreach (var item in FindObjectsOfType<MaskableGraphic>())
            {
                if (item.raycastTarget)
                {
                    Gizmos.color = Color.blue;
                    for (int i = 0; i < 4; i++)
                    {
                        item.rectTransform.GetWorldCorners(boxSize);
                        Gizmos.DrawLine(boxSize[i], boxSize[(i + 1) % 4]);
                    }
                }
            }
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
