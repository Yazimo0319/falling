using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuPanel;      // 主選單
    public GameObject modeSelectPanel;    // 模式選擇畫面

    public void ShowModeSelect()
    {
        mainMenuPanel.SetActive(false);
        modeSelectPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenuPanel.SetActive(true);
        modeSelectPanel.SetActive(false);
    }
}
