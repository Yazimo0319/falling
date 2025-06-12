using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("限時模式背景音樂")]
    public AudioClip[] examBGMs;

    [Header("頒獎 / 悲傷音樂")]
    public AudioClip awardClip;
    public AudioClip sadClip;

    [Header("無盡模式背景音樂")]
    public AudioClip[] endlessBGMs;

    [Header("設定")]
    public float targetVolume = 0.5f;
    public float fadeInDuration = 2f;

    private static BGMManager instance;
    private int currentTrackIndex = -1;
    private bool isEndlessMode = false;
    [Header("結算音樂音量設定")]
    public float resultVolume = 0.3f; // 比 targetVolume 小一點


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        StartCoroutine(WaitAndPlay());
    }

    IEnumerator WaitAndPlay()
    {
        yield return new WaitForSeconds(0.1f); // 等一下 GameManager 實例化

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("BGMManager 找不到 GameManager！");
            yield break;
        }

        string mode = PlayerPrefs.GetString("CurrentMode", "限時");
        isEndlessMode = (mode == "無盡");

        if (isEndlessMode)
        {
            StartCoroutine(PlayEndlessBGM());
        }
        else
        {
            PlayClipWithFade(GetRandomExamBGM());
        }
    }

    void Update()
    {
        if (isEndlessMode && !audioSource.isPlaying && endlessBGMs.Length > 0)
        {
            StartCoroutine(PlayEndlessBGM());
        }
    }

    // 限時模式播放隨機一首
    private AudioClip GetRandomExamBGM()
    {
        if (examBGMs.Length == 0) return null;
        int index = Random.Range(0, examBGMs.Length);
        return examBGMs[index];
    }

    // 無盡模式隨機輪播
    IEnumerator PlayEndlessBGM()
    {
        AudioClip clip = endlessBGMs[Random.Range(0, endlessBGMs.Length)];
        PlayClipWithFade(clip);
        yield return new WaitForSeconds(clip.length + 0.2f);
    }

    public void PlayResultMusic(float score)
{
    // 只在限時模式才播放成績音樂
    if (!isEndlessMode)
    {
        AudioClip resultClip = score >= 80f ? awardClip : sadClip;
        PlayClipWithFade(resultClip, resultVolume); // 傳入結算音量
    }
}


    private void PlayClipWithFade(AudioClip clip, float volume = -1f)
{
    if (clip == null) return;
    StopAllCoroutines();
    float useVolume = (volume >= 0f) ? volume : targetVolume;
    StartCoroutine(FadeInClip(clip, useVolume));
}


    IEnumerator FadeInClip(AudioClip clip, float volume)
{
    audioSource.volume = 0f;
    audioSource.clip = clip;
    audioSource.loop = false;
    audioSource.Play();

    float t = 0f;
    while (t < fadeInDuration)
    {
        t += Time.deltaTime;
        audioSource.volume = Mathf.Lerp(0f, volume, t / fadeInDuration);
        yield return null;
    }
    audioSource.volume = volume;
}

}
