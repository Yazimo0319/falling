using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameOverPanel;
    public TextMeshProUGUI resultText;  // 指向 TMP 的那個「最終結果文字」
    public TextMeshProUGUI scoreText;   // 顯示最終分數
    public int score;

    void Awake()
    {
        instance = this;
    }

    public void GameOver(bool isDead)
    {
        gameOverPanel.SetActive(true);

        if (isDead)
        {
            resultText.text = "遊戲失敗！";
        }
        else
        {
            resultText.text = "🎉 遊戲勝利！";
        }

        scoreText.text = "最終分數：" + score;
    }
}
