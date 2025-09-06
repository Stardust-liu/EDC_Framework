using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameScene : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_EDITOR
        if (!FrameworkManager.isInitFinish)
        {
            FrameworkManager.SetInitFinishLoadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            GetComponentInChildren<BaseEditorStartControl>()?.Init();
            gameObject.SetActive(false);
        }
#else
        gameObject.SetActive(false);
#endif
    }
}
