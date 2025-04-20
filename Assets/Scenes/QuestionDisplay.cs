using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionDisplay : MonoBehaviour
{
    public TMP_Text questionText;
    public Button[] answerButtons;
    private System.Action<string> answerCallback;

    void Start()
    {
        gameObject.SetActive(false);
        Debug.Log("QuestionDisplay initialized on " + gameObject.name);
    }

    public void ShowQuestion(string question, string[] answers, System.Action<string> callback)
    {
        Debug.Log($"Showing question: {question}");
        gameObject.SetActive(true);
        questionText.text = question;
        answerCallback = callback;

        if (answerButtons.Length != 3)
        {
            Debug.LogWarning("Expected 3 answer buttons, but found " + answerButtons.Length);
        }

        string[] shuffledAnswers = new string[answers.Length];
        System.Array.Copy(answers, shuffledAnswers, answers.Length);

        for (int i = shuffledAnswers.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            string temp = shuffledAnswers[i];
            shuffledAnswers[i] = shuffledAnswers[j];
            shuffledAnswers[j] = temp;
        }

        Debug.Log($"Shuffled answers: {string.Join(", ", shuffledAnswers)}");

        for (int i = 0; i < answerButtons.Length && i < shuffledAnswers.Length; i++)
        {
            if (answerButtons[i] == null)
            {
                Debug.LogWarning($"AnswerButton {i} is not assigned!");
                continue;
            }
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = shuffledAnswers[i];
            int index = i;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(shuffledAnswers[index]));
        }
    }

    void OnAnswerSelected(string selectedAnswer)
    {
        Debug.Log($"Answer selected: {selectedAnswer}");
        answerCallback?.Invoke(selectedAnswer);
        // Убрали gameObject.SetActive(false), чтобы канвас оставался активным
    }
}