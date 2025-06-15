using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class RankingManager : MonoBehaviour
{
    [Header("每一列的 UI 元素（手動設定）")]
    public List<TextMeshProUGUI> nameTexts;   // Row1 ~ Row11（第11行顯示玩家）
    public List<TextMeshProUGUI> scoreTexts;  // Row1 ~ Row11

    [Tooltip("使用 'Mock' 或 'Formal' 作為排行榜模式 key")]
    public string modeKey = "Mock";

    void Start()
    {
        LoadAndDisplayRanking();
    }

    void LoadAndDisplayRanking()
    {
        List<PlayerRecord> records = new List<PlayerRecord>();
        int count = PlayerPrefs.GetInt($"{modeKey}_RecordCount", 0);

        for (int i = 0; i < count; i++)
        {
            string prefix = $"{modeKey}_Record_{i}";
            string classText = PlayerPrefs.GetString($"{prefix}_Class", "");
            string seatText = PlayerPrefs.GetString($"{prefix}_Seat", "");
            string nameText = PlayerPrefs.GetString($"{prefix}_Name", "");
            float score = PlayerPrefs.GetFloat($"{prefix}_Score", 0f);

            string userKey = $"{classText}-{seatText}-{nameText}";

            records.Add(new PlayerRecord
            {
                Name = nameText,
                Score = score,
                Index = i,
                UserKey = userKey
            });
        }

        // ✅ 每人保留最高分（若同分則優先較早輸入）
        var bestRecords = records
            .GroupBy(r => r.UserKey)
            .Select(g => g.OrderByDescending(r => r.Score).ThenBy(r => r.Index).First())
            .ToList();

        var topRecords = bestRecords
            .OrderByDescending(r => r.Score)
            .ThenBy(r => r.Index)
            .Take(10)
            .ToList();

        // 顯示 Row1 ~ Row10
        for (int i = 0; i < 10; i++)
        {
            if (i < topRecords.Count)
            {
                nameTexts[i].text = topRecords[i].Name;
                scoreTexts[i].text = $"{topRecords[i].Score:F2}分";
            }
            else
            {
                nameTexts[i].text = "";
                scoreTexts[i].text = "";
            }
        }

        // ✅ 顯示 Row11：玩家最高分對應的名次
        string currentClass = PlayerPrefs.GetString("Class", "");
        string currentSeat = PlayerPrefs.GetString("Seat", "");
        string currentName = PlayerPrefs.GetString("PlayerName", "");
        string currentKey = $"{currentClass}-{currentSeat}-{currentName}";

        var sorted = bestRecords
            .OrderByDescending(r => r.Score)
            .ThenBy(r => r.Index)
            .ToList();

        int playerRank = sorted.FindIndex(r => r.UserKey == currentKey);

        if (playerRank >= 0)
        {
            nameTexts[10].text = $"第 {playerRank + 1} 名";
            scoreTexts[10].text = sorted[playerRank].Score.ToString("F2");
        }
        else
        {
            nameTexts[10].text = "";
            scoreTexts[10].text = "";
        }
    }

    class PlayerRecord
    {
        public string Name;
        public float Score;
        public int Index;
        public string UserKey;
    }
}
