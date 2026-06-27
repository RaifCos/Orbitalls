using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [Header("Audio Tracks")]
    [SerializeField] private AudioClip[] clips; 

    private readonly AudioSource[] sources = new AudioSource[3];

    private const double SCHEDULE_AHEAD_TIME = 1.0;

    void Awake() {
        GameManager.musicManager = this;
        for (int i = 0; i < 3; i++) {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].playOnAwake = false;
            sources[i].loop = true;
        }

        sources[0].volume = 1f;
        sources[1].volume = 0f;
        sources[2].volume = 0f;

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

}