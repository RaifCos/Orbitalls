using UnityEngine;

public class DataManager : MonoBehaviour {

    private void Awake() => GameManager.dataManager = this;

}
