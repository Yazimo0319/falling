using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadFormalExamStart()
    {
        SceneManager.LoadScene("Formal Exam Start");  // 請改成你實際的場景名稱
    }

    public void LoadMockExamStart()
    {
        SceneManager.LoadScene("Mock Exam Start");    // 同上
    }
}
