using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreReportButtons : MonoBehaviour
{
    public void OnRetryMockExam()
    {
        SceneManager.LoadScene("Mock Exam Game");
    }
}
