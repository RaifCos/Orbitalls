using System.Collections;
using UnityEngine;

public class VisualManager : MonoBehaviour {

    [SerializeField] private GameObject titleScreen;
    [SerializeField] private RectTransform titleUI;
    [SerializeField] private GameObject creditScreen;
    [SerializeField] private RectTransform creditUI;
    [SerializeField] private int slideSpeed;
    [SerializeField] private float uiSlideTarget;
    private bool isSliding = false;
    private bool isCredits = false;

    void Awake() { GameManager.visualManager = this; }
    public void StartSliding() { if (!isSliding) StartCoroutine(SlideMenu()); }

    public void CreditRoll() {
        isCredits = true;
        creditScreen.SetActive(true);
        creditUI.anchoredPosition = Vector2.left * uiSlideTarget;
        uiSlideTarget = 0f;
        if (!isSliding) StartCoroutine(SlideMenu());
    }

    IEnumerator SlideMenu() {
        isSliding = true;
        RectTransform targetUI = isCredits ? creditUI : titleUI;
        while (targetUI.anchoredPosition.x > uiSlideTarget) {
            targetUI.anchoredPosition += slideSpeed  * Time.deltaTime * Vector2.left;
            yield return null;
        } 

        if (!isCredits) {
            GameManager.instance.StartGame();
            titleScreen.SetActive(false);
        } 
        
        isSliding = false;
    }
}
