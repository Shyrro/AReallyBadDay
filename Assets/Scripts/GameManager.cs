using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.IO;
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
    private Dictionary<string, object> flags = new Dictionary<string, object>();    

    private void Awake() {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("AmbianceMusic");
    }

    // Start is called before the first frame update
    void Start() {                
        using(var ms = new MemoryStream(Encoding.UTF8.GetBytes(QuestionsFile.text))) {
            var ser = new DataContractJsonSerializer(typeof(Statements), new DataContractJsonSerializerSettings{
                UseSimpleDictionaryFormat = true
            });
            Statements fileData = (Statements)ser.ReadObject(ms);
            AllQuestions = fileData.Questions;        
        }
        
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
        currentQuestionIndex = answer.NextQuestionId;

        if (answer.Change == null || answer.Change.Count == 0) return;

        foreach(var condition in answer.Change) {            
            string label = condition.Key;            
            var value = condition.Value;
            if (flags.ContainsKey(label))
            {
                flags[label] = value;
            }
            else
            {
                flags.Add(label, value);
            }    
        }        
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
                var CurrentAnswer = CurrentQuestion.Answers[i];                                    
                bool activateButton = true;
                var RequiredConditions = CurrentAnswer.Required ?? new Dictionary<string, object>();

                foreach(var condition in RequiredConditions) {
                    flags.TryGetValue(condition.Key, out object conditionValue);  
                    activateButton = conditionValue.Equals(condition.Value);
                    if(!activateButton) break;
                }                     
                
                if (activateButton)
                {
                    ButtonTexts[i].text = CurrentAnswer.Label;
                    AnswerButtons[i].gameObject.SetActive(true);
                }
            }            

            string imageString = !string.IsNullOrWhiteSpace(CurrentQuestion.Image) ? CurrentQuestion.Image : "Sprites/Backgrounds/black";
            BackgroundImage.sprite = Resources.Load<Sprite>(imageString);
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