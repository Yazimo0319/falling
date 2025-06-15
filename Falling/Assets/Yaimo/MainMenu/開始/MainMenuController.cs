using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("面板")]
    public GameObject mainMenuPanel;          // 主選單畫面
    public GameObject gameModeSelectPanel;    // 遊戲模式選擇畫面
    public GameObject rankingModeSelectPanel; // 排行榜模式選擇畫面

    // 👉 開始遊戲 → 顯示遊戲模式選擇畫面
    public void ShowGameModeSelect()
    {
        mainMenuPanel.SetActive(false);
        gameModeSelectPanel.SetActive(true);
    }

    // 👉 查看排行榜 → 顯示排行榜選擇畫面
    public void ShowRankingModeSelect()
    {
        mainMenuPanel.SetActive(false);
        rankingModeSelectPanel.SetActive(true);
    }

    // 👉 選擇遊戲模式後切換場景
    public void GoToFormalExam()
    {
        PlayerPrefs.SetString("CurrentMode", "限時");
        SceneManager.LoadScene("Formal Exam Start");
    }

    public void GoToMockExam()
    {
        PlayerPrefs.SetString("CurrentMode", "無盡");
        SceneManager.LoadScene("Mock Exam Start");
    }

    // 👉 選擇排行榜模式後切換場景
    public void GoToFormalRanking()
    {
        SceneManager.LoadScene("FormalRankingScene");
    }

    public void GoToMockRanking()
    {
        SceneManager.LoadScene("MockRankingScene");
    }

    // 👉 返回主選單
    public void BackToMainMenu()
    {
        mainMenuPanel.SetActive(true);
        gameModeSelectPanel.SetActive(false);
        rankingModeSelectPanel.SetActive(false);
    }
}
