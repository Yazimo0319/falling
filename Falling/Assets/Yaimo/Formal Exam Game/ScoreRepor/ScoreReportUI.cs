using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreReportUI : MonoBehaviour
{
    public TextMeshProUGUI classText, seatText, nameText, scoreText;
    public Image commentImage;

    public Sprite comment_優, comment_甲, comment_乙, comment_丙;

    public void Show(float score)
{
    classText.text = PlayerPrefs.GetString("Class", "二乙");
    seatText.text = PlayerPrefs.GetString("Seat", "14");
    nameText.text = PlayerPrefs.GetString("PlayerName", "小藍");
    scoreText.text = $"{score:F2}";

    string mode = PlayerPrefs.GetString("CurrentMode", "限時");

    if (mode == "限時")
    {
        if (commentImage != null)
        {
            commentImage.gameObject.SetActive(true);

            if (score >= 90f) commentImage.sprite = comment_優;
            else if (score >= 80f) commentImage.sprite = comment_甲;
            else if (score >= 70f) commentImage.sprite = comment_乙;
            else commentImage.sprite = comment_丙;
        }
    }
    else
    {
        if (commentImage != null)
        {
            commentImage.gameObject.SetActive(false); // 模擬考不顯示
        }
    }

    gameObject.SetActive(true);
}


}
