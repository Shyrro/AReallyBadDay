using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public TextAsset QuestionsFile;
    public TextMeshProUGUI QuestionText;
    public List<TextMeshProUGUI> ButtonTexts;
    public List<Button> AnswerButtons;
    public Image BackgroundImage;
    private Statement[] AllQuestions;
    public int currentQuestionIndex = 0;
    private bool currentTextAlreadyFilled;

    // Start is called before the first frame update
    void Start() {
        Statements fileData = JsonUtility.FromJson<Statements>(QuestionsFile.text);
        AllQuestions = fileData.Questions;
    }

    // Update is called once per frame
    void Update() {
        FillUITexts();
    }

    public void AnswerQuestion(int answerId) {
        var currentQuestion = AllQuestions[currentQuestionIndex];

        if(currentQuestion.Failure) {            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  
            return;          
        }        

        if(currentQuestion.Success) {
            // TODO: Go to winning scene
        }

        ChangeQuestion(currentQuestion.Answers[answerId].NextQuestionId);
    }

    private void ChangeQuestion(int goToQuestionId) {
        currentTextAlreadyFilled = false;
        currentQuestionIndex = goToQuestionId;
    }

    private void FillUITexts() {
        if (currentQuestionIndex < AllQuestions.Length) {
            var currentQuestion = AllQuestions[currentQuestionIndex];
            if (!currentTextAlreadyFilled) {
                QuestionText.text = currentQuestion.Question;
                for (var i = 0; i < currentQuestion.Answers.Length; i++) {
                    ButtonTexts[i].text = currentQuestion.Answers[i].Label;
                    AnswerButtons[i].gameObject.SetActive(true);
                }
                
                Sprite background = Resources.Load<Sprite>(currentQuestion.Image);                        
                BackgroundImage.sprite = background;                
                currentTextAlreadyFilled = true;
            }
        }
    }

}