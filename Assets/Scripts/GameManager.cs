using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public static GameplayManager gameplayManager;

    void Awake() => instance = this; 
}
