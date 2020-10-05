using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
    
    private AudioManager audioManager;
    public GameObject tutorialPanel;

    private void Awake() {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlayButton(){
        audioManager.Play("buttonClick");
        audioManager.Stop("MainScreenMusic");
        SceneManager.LoadScene(1);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void TutorialButton() {
        audioManager.Play("buttonClick");        
        tutorialPanel.SetActive(true);
    }

    public void CloseTutorialPanel(){
        audioManager.Play("buttonClick");        
        tutorialPanel.SetActive(false);
    }

}