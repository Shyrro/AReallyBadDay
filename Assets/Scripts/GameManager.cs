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

    // Start is called before the first frame update
    void Start() {
        Statements fileData = JsonUtility.FromJson<Statements>(QuestionsFile.text);
        AllQuestions = fileData.Questions;
        audioManager = FindObjectOfType<AudioManager>();
        HideButtons();
    }

    // Update is called once per frame
    void Update() {
        FillUITexts();
    }

    public void AnswerQuestion(int answerId) {
        if (CurrentQuestion.Failure) {
            Replay();
            return;
        }

        if (CurrentQuestion.Success) {
            SceneHelper.GoToSuccessScene();
            return;
        }

        audioManager.Play("buttonClick");
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
            QuestionText.text = CurrentQuestion.Question;
            for (var i = 0; i < CurrentQuestion.Answers.Length; i++) {
                ButtonTexts[i].text = CurrentQuestion.Answers[i].Label;
                AnswerButtons[i].gameObject.SetActive(true);
            }

            if (!string.IsNullOrEmpty(CurrentQuestion.Image)) {
                Sprite background = Resources.Load<Sprite>(CurrentQuestion.Image);
                BackgroundImage.sprite = background;
            }

            currentTextAlreadyFilled = true;
        }
    }
}