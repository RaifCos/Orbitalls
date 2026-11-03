using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour {

    [SerializeField] private Button[] levelButtons;
    [SerializeField] private GameObject levelSelectCanvas;

    private void Awake() { 
        GameManager.levelSelectManager = this;  
    }

    public void UpdateAllButtons(int highest) {
        for (int i = 0; i < levelButtons.Length; i++) { levelButtons[i].interactable = i <= highest; }
    }

    public void UpdateButtons(int highest) {
        for (int i = 0; i < highest; i++) { levelButtons[i].interactable = true; }
    }

    public void SelectLevel(int lvl) {
        levelSelectCanvas.SetActive(false);
        GameManager.instance.SelectLevel(lvl - 1);
    }
}
