using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public TextAsset QuestionsFile;
    public TextMeshProUGUI QuestionText;
    public List<TextMeshProUGUI> ButtonTexts;
    private Statement[] AllQuestions;
    private List<bool> questionsAnswered;
    public int currentQuestionIndex = 0;
    private bool currentTextAlreadyFilled;

    // Start is called before the first frame update
    void Start() {
        Statements fileData = JsonUtility.FromJson<Statements>(QuestionsFile.text);
        AllQuestions = fileData.Questions;
        questionsAnswered = new List<bool>(new bool[AllQuestions.Length + 1]);        
    }

    // Update is called once per frame
    void Update() {
        if (currentQuestionIndex < AllQuestions.Length) {
            var currentQuestion = AllQuestions[currentQuestionIndex];
            if (!currentTextAlreadyFilled) {
                QuestionText.text = currentQuestion.Question;
                for (var i = 0; i < currentQuestion.Answers.Length; i++) {
                    ButtonTexts[i].text = currentQuestion.Answers[i].Label;
                }
                currentTextAlreadyFilled = true;
            }
        }
    }

    public void AnswerQuestion(int answerId) {
        var currentQuestion = AllQuestions[currentQuestionIndex];

        if(currentQuestion.Failure) {            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

}