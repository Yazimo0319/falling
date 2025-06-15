using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoMockRanking : MonoBehaviour
{
    public string rankingSceneName = "MockRankingScene";

    public void GoToRanking()
    {
        SceneManager.LoadScene(rankingSceneName);
    }
}
