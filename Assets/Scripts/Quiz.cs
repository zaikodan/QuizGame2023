using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quiz 1", menuName = "Question")]
public class Quiz : ScriptableObject
{
    [SerializeField] string question;
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswer;

    public enum CategoryEnum { Matematica, Portugues, Historia, Geografia, Quimica }
    public enum DifficultyEnum { Facil, Medio, Dificil}

    [SerializeField] CategoryEnum category;
    [SerializeField] DifficultyEnum difficulty;

    const int scoreEasy = 100, scoreMedium = 200, scoreHard = 300;

    public int Score
    {
        get
        {
            switch (difficulty)
            {
                case DifficultyEnum.Facil:
                    return scoreEasy;

                case DifficultyEnum.Medio:
                    return scoreMedium;

                case DifficultyEnum.Dificil:
                    return scoreHard;
            }

            return 0;
        }
    }
    public string Question { get => question; }
    public string[] Answers { get => answers; }
    public int CorrectAnswer { get => correctAnswer; }
    public CategoryEnum Category { get => category; }
    public DifficultyEnum Difficulty { get => difficulty; }
}
