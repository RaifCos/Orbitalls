using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour {

    [SerializeField] private Button[] levelButtons;
    [SerializeField] private GameObject levelSelectCanvas;

    private void Awake() { 
        GameManager.levelSelectManager = this;  
    }
    
    private void UpdateButton(int lvl, int highest) {
        levelButtons[lvl].interactable = lvl <= highest;
    }

    public void UpdateAllButtons(int highest) {
        for (int i = 0; i < levelButtons.Length; i++) {
            UpdateButton(i, highest);
        }
    }

    public void SelectLevel(int lvl) {
        levelSelectCanvas.SetActive(false);
        GameManager.instance.SelectLevel(lvl - 1);
    }
}
