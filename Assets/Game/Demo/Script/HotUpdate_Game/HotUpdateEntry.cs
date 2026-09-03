using Cysharp.Threading.Tasks;
using UnityEngine;

public static class HotUpdateEntry
{
    private const string EnterGameScene = "EnterGameScene";

    private static GameStartInfo startInfo;

    public static async UniTask Init(GameStartInfo startInfo)
    {
        HotUpdateEntry.startInfo = startInfo;
        await GameModule.Init();
    }

    public static void ReadyRegisteredModules()
    {
        GameModule.ReadyRegisteredModules();
    }

    public static async UniTask EnterGame()
    {
        var sceneName = GetEnterGameSceneName();

        await Hub.Scene.LoadScene(sceneName);
    }

    public static void Quit()
    {
        GameModule.Quit();
    }

    /// <summary>
    /// 获取进入游戏场景
    /// </summary>
    private static string GetEnterGameSceneName()
    {
        return startInfo.HasTargetScene
            ? startInfo.TargetSceneName
            : EnterGameScene;
    }
}
