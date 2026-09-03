using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.SceneManagement;
#endif

[DefaultExecutionOrder(-10000)]
public class GameScene : MonoBehaviour
{
#if UNITY_EDITOR
    private void Awake()
    {
        if (!FrameworkManager.isInitFinish)
        {
            FrameworkManager.SetInitFinishLoadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("MainScene");
            return;
        }
        gameObject.SetActive(false);
    }
#endif
}
