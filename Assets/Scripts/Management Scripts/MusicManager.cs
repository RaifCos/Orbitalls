using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [Header("Audio Tracks")]
    [SerializeField] private AudioClip[] clips; 

    private readonly AudioSource[] sources = new AudioSource[3];

    private const double SCHEDULE_AHEAD_TIME = 1.0;
    private int musicPhase = -1;

    void Awake() {
        GameManager.musicManager = this;
        for (int i = 0; i < 3; i++) {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].playOnAwake = false;
            sources[i].loop = true;
        }
        SetMusicPhase(0);
        for (int i = 0; i < 3; i++) { sources[i].clip = clips[i]; }
    }

    public void StartPlaying() { PlayAllSynced(); }

    public void PlayAllSynced() {
        double startTime = AudioSettings.dspTime + SCHEDULE_AHEAD_TIME;
        foreach (var source in sources) { source.PlayScheduled(startTime); }
    }

    public void StopAll() { foreach (var source in sources) { source.Stop(); } }

    public IEnumerator FadeMusicIn(int phase) {
        AudioSource aS = sources[phase];
        while(aS.volume < 1f) {
            aS.volume += 0.01f;
            yield return null;
        }
    }

    public void MuteMusic(int phase) {
        AudioSource aS = sources[phase];
        aS.volume = 0f;
    }

    public void SetMusicPhase(int p) {
        Debug.Log(p + " " + musicPhase);
        if (p == musicPhase) return;
        musicPhase = p;

        StopAllCoroutines();
        switch(musicPhase) {
            case 0:
                MuteMusic(1);
                MuteMusic(2);
                break;
            case 1:
                StartCoroutine(FadeMusicIn(1));
                MuteMusic(2);
                break;
            case 2:
                StartCoroutine(FadeMusicIn(1));
                StartCoroutine(FadeMusicIn(2));
                break;
        }
    }
}