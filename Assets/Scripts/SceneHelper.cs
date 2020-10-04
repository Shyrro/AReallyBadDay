using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour{
    
    public static void GoToMainScene() {
        SceneManager.LoadScene(0);  
    }    

    public static void GoToSuccessScene() {
        SceneManager.LoadScene("SuccessScene"); 
    }
}