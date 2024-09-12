using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
   private void Awake(){
        if(!FrameworkManager.isInitFinish){
            FrameworkManager.SetInitFinishLoadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("MainScene");
        }
        else{
            gameObject.SetActive(false);
        }
   }
}
