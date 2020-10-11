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
    //A dictionary to store flags as they come
    private Dictionary<string, bool> flags = new Dictionary<string, bool>();

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
        
        ChangeQuestion(CurrentQuestion.Answers[answerId]);
    }

    public void Replay() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ChangeQuestion(Answer answer) {
        currentTextAlreadyFilled = false;
        HideButtons();
        //test for missing key
        if (answer.Change.Key != null)
        {
            string label = answer.Change.Key;
            bool value = answer.Change.BoolValue;
            //chek if flag was added to dictionary else add it
            if (flags.ContainsKey(label))
            {
                flags[label] = value;
            }
            else
            {
                flags.Add(label, value);
            }
        }
        currentQuestionIndex = answer.NextQuestionId;
    }

    private void HideButtons(){
        for (var i = 0; i < AnswerButtons.Count; i++) {
            AnswerButtons[i].gameObject.SetActive(false);
        }
    }

    private void FillUITexts() {
        if (!currentTextAlreadyFilled) {
            Statement CurrQuestion = CurrentQuestion;
            Answer CurrAnswer;
            Condition CurrCondition;
            bool activateButton;
            StopAllCoroutines();
            StartCoroutine(TypeSentence(CurrQuestion.Question));
            for (var i = 0; i < CurrQuestion.Answers.Length; i++) {
                CurrAnswer = CurrQuestion.Answers[i];
                CurrCondition = CurrAnswer.Required;
                activateButton = false;
                // check for mising key
                if (CurrCondition.Key != null)
                {
                    //check if key is present in dictionary
                    if (flags.ContainsKey(CurrCondition.Key))
                    {
                        //check if flag value matches required value
                        if (flags[CurrCondition.Key] == CurrCondition.BoolValue)
                        {
                            activateButton = true;
                        }
                    }
                    else
                    {
                        activateButton = true;
                    }
                }
                else
                {
                    activateButton = true;
                }

                if (activateButton)
                {
                    ButtonTexts[i].text = CurrAnswer.Label;
                    AnswerButtons[i].gameObject.SetActive(true);
                }
            }

            if (!string.IsNullOrWhiteSpace(CurrQuestion.Image)) {
                Sprite background = Resources.Load<Sprite>(CurrQuestion.Image);
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
            QuestionText.text += letter;
            yield return null;
        }
    }
}