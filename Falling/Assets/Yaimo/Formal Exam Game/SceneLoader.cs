using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadFormalRanking()
    {
        SceneManager.LoadScene("FormalRankingScene"); // 替換為你的限時榜單場景名稱
    }

    public void LoadMockRanking()
    {
        SceneManager.LoadScene("MockRankingScene"); // 替換為模擬榜單的場景名稱
    }
}
