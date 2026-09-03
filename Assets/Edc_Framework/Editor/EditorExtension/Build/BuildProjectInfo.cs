using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildProjectInfo", menuName = "创建.Assets文件/FrameworkTool/BuildProjectInfo")]
public class BuildProjectInfo : ScriptableObject
{
    public const string AssetPath = "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Build/BuildProjectInfo.asset";

    [PropertyOrder(0)]
    [OnInspectorGUI]
    private void DrawProjectInfo()
    {
        EditorGUILayout.HelpBox("用于管理会影响应用身份和基础运行方向的 Unity PlayerSettings 字段。产品名称会影响 Application.productName、窗口标题和部分平台的存档路径。", MessageType.Info);
    }

    [PropertyOrder(1)]
    [Header("应用身份")]
    [LabelText("公司名称")]
    [Tooltip("写入 PlayerSettings.companyName，会影响 Application.companyName 和部分平台的存档路径。")]
    public string companyName = "DefaultCompany";

    [PropertyOrder(1)]
    [LabelText("产品名称")]
    [Tooltip("写入 PlayerSettings.productName，会影响 Application.productName、窗口标题和部分平台的存档路径。")]
    public string productName = "EDC_Framework";

    [PropertyOrder(2)]
    [Header("屏幕方向")]
    [LabelText("默认屏幕方向")]
    [Tooltip("写入 PlayerSettings.defaultInterfaceOrientation，用来控制应用启动后的默认横竖屏方向。")]
    public UIOrientation defaultInterfaceOrientation = UIOrientation.AutoRotation;

    [PropertyOrder(2)]
    [ShowIf(nameof(IsAutoRotation))]
    [LabelText("允许竖屏")]
    public bool allowPortrait = true;

    [PropertyOrder(2)]
    [ShowIf(nameof(IsAutoRotation))]
    [LabelText("允许倒竖屏")]
    public bool allowPortraitUpsideDown;

    [PropertyOrder(2)]
    [ShowIf(nameof(IsAutoRotation))]
    [LabelText("允许左横屏")]
    public bool allowLandscapeLeft = true;

    [PropertyOrder(2)]
    [ShowIf(nameof(IsAutoRotation))]
    [LabelText("允许右横屏")]
    public bool allowLandscapeRight = true;


    [PropertyOrder(3)]
    [Button("应用项目基础信息", ButtonSizes.Large), GUIColor(0.5f, 0.8f, 1f)]
    private void ApplyProjectInfoFromInspector()
    {
        if (ApplyProjectInfo())
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }

    public static BuildProjectInfo LoadAsset()
    {
        return AssetDatabase.LoadAssetAtPath<BuildProjectInfo>(AssetPath);
    }


    public bool ApplyProjectInfo()
    {
        ValidateAutoRotationDirections();

        var nextCompanyName = GetConfiguredCompanyName();
        var nextProductName = GetConfiguredProductName();
        var isChanged = false;

        if (!string.Equals(PlayerSettings.companyName, nextCompanyName, StringComparison.Ordinal))
        {
            PlayerSettings.companyName = nextCompanyName;
            isChanged = true;
        }

        if (!string.Equals(PlayerSettings.productName, nextProductName, StringComparison.Ordinal))
        {
            PlayerSettings.productName = nextProductName;
            isChanged = true;
        }

        if (PlayerSettings.defaultInterfaceOrientation != defaultInterfaceOrientation)
        {
            PlayerSettings.defaultInterfaceOrientation = defaultInterfaceOrientation;
            isChanged = true;
        }

        if (PlayerSettings.allowedAutorotateToPortrait != allowPortrait)
        {
            PlayerSettings.allowedAutorotateToPortrait = allowPortrait;
            isChanged = true;
        }

        if (PlayerSettings.allowedAutorotateToPortraitUpsideDown != allowPortraitUpsideDown)
        {
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = allowPortraitUpsideDown;
            isChanged = true;
        }

        if (PlayerSettings.allowedAutorotateToLandscapeLeft != allowLandscapeLeft)
        {
            PlayerSettings.allowedAutorotateToLandscapeLeft = allowLandscapeLeft;
            isChanged = true;
        }

        if (PlayerSettings.allowedAutorotateToLandscapeRight != allowLandscapeRight)
        {
            PlayerSettings.allowedAutorotateToLandscapeRight = allowLandscapeRight;
            isChanged = true;
        }

        if (isChanged)
        {
            Debug.Log($"已应用项目基础信息：{nextCompanyName} / {nextProductName} / {defaultInterfaceOrientation}");
        }

        return true;
    }

    public string GetConfiguredProductName()
    {
        return string.IsNullOrWhiteSpace(productName)
            ? (string.IsNullOrWhiteSpace(PlayerSettings.productName) ? "New Unity Project" : PlayerSettings.productName)
            : productName.Trim();
    }

    private void OnEnable()
    {
        FillFromPlayerSettingsIfEmpty();
        ValidateAutoRotationDirections();
    }

    private void OnValidate()
    {
        FillFromPlayerSettingsIfEmpty();
        ValidateAutoRotationDirections();
    }

    private bool IsAutoRotation()
    {
        return defaultInterfaceOrientation == UIOrientation.AutoRotation;
    }

    private string GetConfiguredCompanyName()
    {
        return string.IsNullOrWhiteSpace(companyName)
            ? (string.IsNullOrWhiteSpace(PlayerSettings.companyName) ? "DefaultCompany" : PlayerSettings.companyName)
            : companyName.Trim();
    }

    private void FillFromPlayerSettingsIfEmpty()
    {
        if (string.IsNullOrWhiteSpace(companyName))
        {
            companyName = string.IsNullOrWhiteSpace(PlayerSettings.companyName) ? "DefaultCompany" : PlayerSettings.companyName;
        }

        if (string.IsNullOrWhiteSpace(productName))
        {
            productName = string.IsNullOrWhiteSpace(PlayerSettings.productName) ? "New Unity Project" : PlayerSettings.productName;
        }
    }

    private void ValidateAutoRotationDirections()
    {
        if (defaultInterfaceOrientation != UIOrientation.AutoRotation)
        {
            return;
        }

        if (allowPortrait || allowPortraitUpsideDown || allowLandscapeLeft || allowLandscapeRight)
        {
            return;
        }

        allowPortrait = true;
        allowLandscapeLeft = true;
    }
}