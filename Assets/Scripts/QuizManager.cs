using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
   [SerializeField] Text questionText, resultText;
   [SerializeField] Text[] answersText;
   [SerializeField] Quiz[] quizList;
   [SerializeField] GameObject resultPanel, menuPanel;
   [SerializeField] Toggle[] toggles;
   [SerializeField] Dropdown difficultySelected;
    [SerializeField] Image[] answersButtons;
    [SerializeField] Button playButton;
    [SerializeField] InputField inputName;

    [SerializeField] SaveSystem saveSystem;


   bool[] quizAskeds;

    int quizSelected;
    int result;
    int score;
    int quizAskedsAmount;
    int playersAmount;
    const int delayGenerateQuiz = 1;

    private void Start()
    {
        playButton.interactable = false;
        inputName.text = null;
        for(int i = 0; i < toggles.Length; i++)
        {
            toggles[i].isOn = false;
        }

        playersAmount = PlayerPrefs.GetInt("PlayersAmount");
    }


    void UpdateQuiz()
    {
        questionText.text = quizList[quizSelected].Question;
        for (int i = 0; i < answersText.Length; i++)
        {
            answersText[i].text = quizList[quizSelected].Answers[i];
            answersButtons[i].color = Color.white;
        }
    }
    
    bool ValidateQuiz(int _quizSelected)
    {
        return toggles[(int)quizList[_quizSelected].Category].isOn && difficultySelected.value == (int)quizList[_quizSelected].Difficulty ;
    }

    void GenerateQuiz()
    {
  
        quizSelected = Random.Range(0, quizList.Length);
        if (ValidateQuiz(quizSelected)) 
        {
            if (quizAskeds[quizSelected])
            {
                CheckQuizAskeds();
            }
            else
            {
                quizAskeds[quizSelected] = true;
                UpdateQuiz();
                quizAskedsAmount++;
            }

        }
        else
        {
            GenerateQuiz();
        }

    }

    void CheckQuizAskeds()
    {
        for(int i = 0; i < quizAskeds.Length; i++)
        {
            if (!quizAskeds[i] && ValidateQuiz(i))
            {
                break;
            }
            else
            {
                if(i == quizAskeds.Length - 1)
                {
                    ShowResult();
                    return;
                }
            }
        }

        GenerateQuiz();
    }

    private void ShowResult()
    {
        resultPanel.SetActive(true);
        resultText.text = "Jogador: " + inputName.text + "\n" + "Acertou: " + result + " de " + quizAskedsAmount + " questões" + "\n" + "Pontos: " + score.ToString("0000");
        saveSystem.SavePlayer(inputName.text, score);
    }

    public void CheckAnswer(int answerSelected)
    {
        if (!answered)
        {
            StartCoroutine(CheckAnswerCoroutine(answerSelected));
        }
    }

    bool answered;
        
    private IEnumerator CheckAnswerCoroutine(int answerSelected)
    {
        answered = true;
        if (answerSelected == quizList[quizSelected].CorrectAnswer)
        {
            Debug.Log("Acertou");
            answersButtons[answerSelected].color = Color.green;
            result++;
            score += quizList[quizSelected].Score;
        }
        else
        {
            Debug.Log("Errou");
            answersButtons[answerSelected].color = Color.red;
            answersButtons[quizList[quizSelected].CorrectAnswer].color = Color.green;
        }

        yield return new WaitForSeconds(delayGenerateQuiz);
        GenerateQuiz();
        answered = false;
    }

    private bool CheckToogles()
    {
        for(int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                return true;
            }
        }

        return false;
    }

    public void CheckStart()
    {

        playButton.interactable = CheckToogles() && !string.IsNullOrEmpty(inputName.text);
    }

    public void ResetGame()
    {
        resultPanel.SetActive(false);
        result = 0;
        score = 0;
        quizAskedsAmount = 0;
        quizAskeds = new bool[quizList.Length];
        GenerateQuiz();

    }

    public void StartGame()
    {
        menuPanel.SetActive(false);
        ResetGame();
    }

    public void ReturnToMenu()
    {
        menuPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
