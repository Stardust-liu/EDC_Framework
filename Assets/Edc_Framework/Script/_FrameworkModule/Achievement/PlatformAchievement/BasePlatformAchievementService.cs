/// <summary>
/// 平台成就服务基类，统一负责监听框架成就事件。
/// </summary>
public abstract class BasePlatformAchievementService : IBindEvent
{
    private bool isPlatformInitialized;
    private bool isUnlockListenerRegistered;
    private bool isProgressListenerRegistered;

    public void Init()
    {
        if (isPlatformInitialized)
        {
            return;
        }

        if (!InitializePlatform())
        {
            LogManager.LogWarning($"{GetType().Name} 初始化失败，平台成就同步未启用");
            return;
        }

        isPlatformInitialized = true;
        try
        {
            this.AddListener<UnlockAchievement>(OnUnlockAchievementEvent);
            isUnlockListenerRegistered = true;
            this.AddListener<UpdateAchievementSchedule>(OnUpdateAchievementScheduleEvent);
            isProgressListenerRegistered = true;
        }
        catch
        {
            Uninstall();
            throw;
        }
    }

    public void Uninstall()
    {
        if (isUnlockListenerRegistered)
        {
            this.RemoveListener<UnlockAchievement>(OnUnlockAchievementEvent);
            isUnlockListenerRegistered = false;
        }

        if (isProgressListenerRegistered)
        {
            this.RemoveListener<UpdateAchievementSchedule>(OnUpdateAchievementScheduleEvent);
            isProgressListenerRegistered = false;
        }

        if (!isPlatformInitialized)
        {
            return;
        }

        isPlatformInitialized = false;
        UninstallPlatform();
    }

    private void OnUnlockAchievementEvent(UnlockAchievement unlockAchievement)
    {
        UnlockPlatformAchievement(unlockAchievement.unlockAchievement);
    }

    private void OnUpdateAchievementScheduleEvent(UpdateAchievementSchedule updateAchievementSchedule)
    {
        UpdatePlatformAchievementProgress(updateAchievementSchedule.unlockAchievement, updateAchievementSchedule.schedule);
    }

    /// <summary>
    /// 初始化具体平台 SDK 或平台成就服务。
    /// </summary>
    protected abstract bool InitializePlatform();

    /// <summary>
    /// 获得成就
    /// </summary>
    protected abstract void UnlockPlatformAchievement(string achievementID);

    /// <summary>
    /// 更新成就进度
    /// </summary>
    protected abstract void UpdatePlatformAchievementProgress(string achievementID, int progress);

    /// <summary>
    /// 清理具体平台服务。
    /// </summary>
    protected virtual void UninstallPlatform() { }
}