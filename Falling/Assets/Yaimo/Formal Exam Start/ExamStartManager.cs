using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ExamStartManager : MonoBehaviour
{
    [Header("輸入欄位")]
    public TMP_InputField inputClass;
    public TMP_InputField inputSeat;
    public TMP_InputField inputName;

    [Header("考試模式設定")]
    [Tooltip("請輸入模式名稱，例如 '限時' 或 '無盡'")]
    public string modeName = "限時";

    [Tooltip("完成輸入後要切換的場景名稱")]
    public string sceneToLoad = "Formal Exam Game";

    public void OnStartButtonClick()
    {
        string classText = inputClass.text.Trim();
        string seatText = inputSeat.text.Trim();
        string nameText = inputName.text.Trim();

        // 防呆：欄位不能空
        if (string.IsNullOrEmpty(classText) || string.IsNullOrEmpty(seatText) || string.IsNullOrEmpty(nameText))
        {
            Debug.LogWarning("⚠️ 請完整填寫班級、座號、姓名！");
            return;
        }

        // 儲存輸入資料
        PlayerPrefs.SetString("Class", classText);
        PlayerPrefs.SetString("Seat", seatText);
        PlayerPrefs.SetString("PlayerName", nameText);
        PlayerPrefs.SetString("CurrentMode", modeName);

        Debug.Log($"✅ 開始作答 - 模式：{modeName}，玩家：{classText}-{seatText}-{nameText}");

        // 進入指定場景
        SceneManager.LoadScene(sceneToLoad);
    }
}
