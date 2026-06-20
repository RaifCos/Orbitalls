using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public static GameplayManager gameplayManager;
    public static DataManager dataManager;

    void Awake() => instance = this; 
}
