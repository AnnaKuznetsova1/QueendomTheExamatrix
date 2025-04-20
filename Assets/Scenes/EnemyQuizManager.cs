using UnityEngine;

public class EnemyQuizManager : MonoBehaviour
{
    public QuestionInputManager questionInputManager;
    public GameObject questionCanvas;
    public int bossIndex;
    private bool hasAnswered = false;

    void Start()
    {
        if (questionInputManager == null)
        {
            Debug.LogWarning("QuestionInputManager is not assigned in EnemyQuizManager on " + gameObject.name);
        }
        if (questionCanvas == null)
        {
            Debug.LogWarning("QuestionCanvas is not assigned in EnemyQuizManager on " + gameObject.name);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasAnswered)
        {
            Debug.Log($"{gameObject.name} collided with Player!");
            AudioManager.Instance.PlayBossMusic(); // Воспроизводим музыку босса
            ShowQuestion();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasAnswered)
        {
            Debug.Log($"{gameObject.name}: Player exited trigger zone. Resetting hasAnswered.");
            hasAnswered = false;
            if (questionCanvas != null)
            {
                questionCanvas.SetActive(false);
                Debug.Log("QuestionCanvas deactivated on trigger exit.");
            }
            AudioManager.Instance.StopBossMusic(); // Останавливаем музыку босса, если игрок ушёл
        }
    }

    void ShowQuestion()
    {
        if (questionInputManager != null)
        {
            questionInputManager.ShowQuestion(bossIndex, OnAnswerReceived);
        }
        else
        {
            Debug.LogWarning("Cannot show question: QuestionInputManager is null on " + gameObject.name);
        }
    }

    void OnAnswerReceived(string selectedAnswer)
    {
        if (hasAnswered)
        {
            Debug.Log($"{gameObject.name}: Answer already received, ignoring.");
            return;
        }

        hasAnswered = true;
        Debug.Log($"{gameObject.name} received answer: {selectedAnswer}");

        bool isCorrect = questionInputManager.CheckAnswer(bossIndex, selectedAnswer);
        if (isCorrect)
        {
            Debug.Log($"Correct answer! {gameObject.name} defeated.");
            AudioManager.Instance.PlayCorrectAnswerSound(); // Звук правильного ответа
            if (questionCanvas != null)
            {
                questionCanvas.SetActive(false);
                Debug.Log("QuestionCanvas deactivated after correct answer.");
            }
            AudioManager.Instance.StopBossMusic(); // Останавливаем музыку босса
            Destroy(gameObject);
            FindObjectOfType<GameFlowManager>().OnBossDefeated();
        }
        else
        {
            Debug.Log("Wrong answer! Player loses a life.");
            AudioManager.Instance.PlayWrongAnswerSound(); // Звук неправильного ответа
            hasAnswered = false;
            var gameFlowManager = FindObjectOfType<GameFlowManager>();
            if (gameFlowManager != null)
            {
                gameFlowManager.LoseLife();
            }
            else
            {
                Debug.LogWarning("GameFlowManager not found! Cannot decrease lives.");
            }
        }
    }
}