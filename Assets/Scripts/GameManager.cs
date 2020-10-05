using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public TextAsset QuestionsFile;
    public TextMeshProUGUI QuestionText;
    public List<TextMeshProUGUI> ButtonTexts;
    public List<Button> AnswerButtons;
    public Image BackgroundImage;
    private Statement[] AllQuestions;
    public int currentQuestionIndex = 0;
    private bool currentTextAlreadyFilled;
    private AudioManager audioManager;

    private Statement CurrentQuestion => AllQuestions.First(x => x.Id == currentQuestionIndex);

    private void Awake() {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("AmbianceMusic");
    }
    // Start is called before the first frame update
    void Start() {        
        Statements fileData = JsonUtility.FromJson<Statements>(QuestionsFile.text);
        AllQuestions = fileData.Questions;        
        HideButtons();
    }

    // Update is called once per frame
    void Update() {
        FillUITexts();
    }

    public void AnswerQuestion(int answerId) {
        audioManager.Play("buttonClick");
        if (CurrentQuestion.Failure) {
            Replay();
            return;
        }

        if (CurrentQuestion.Success) {
            SceneHelper.GoToMainScene();
            return;
        }
        
        ChangeQuestion(CurrentQuestion.Answers[answerId].NextQuestionId);
    }

    public void Replay() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ChangeQuestion(int goToQuestionId) {
        currentTextAlreadyFilled = false;
        HideButtons();
        currentQuestionIndex = goToQuestionId;
    }

    private void HideButtons(){
        for (var i = 0; i < AnswerButtons.Count; i++) {
            AnswerButtons[i].gameObject.SetActive(false);
        }
    }

    private void FillUITexts() {
        if (!currentTextAlreadyFilled) {
            StopAllCoroutines();
            StartCoroutine(TypeSentence(CurrentQuestion.Question));
            for (var i = 0; i < CurrentQuestion.Answers.Length; i++) {
                ButtonTexts[i].text = CurrentQuestion.Answers[i].Label;
                AnswerButtons[i].gameObject.SetActive(true);
            }

            if (!string.IsNullOrWhiteSpace(CurrentQuestion.Image)) {
                Sprite background = Resources.Load<Sprite>(CurrentQuestion.Image);
                BackgroundImage.sprite = background;
            }else {
                Sprite background = Resources.Load<Sprite>("Sprites/Backgrounds/black");
                BackgroundImage.sprite = background;
            }

            currentTextAlreadyFilled = true;
        }
    }

    IEnumerator TypeSentence(string sentence) {
        QuestionText.text = string.Empty;
        foreach(char letter in sentence.ToCharArray()){            
            audioManager.Play("keyboard");
            QuestionText.text += letter;
            yield return new WaitForSeconds(.07f);
        }
    }
}