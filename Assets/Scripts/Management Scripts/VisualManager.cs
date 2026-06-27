using System.Collections;
using UnityEngine;
using TMPro;

public class VisualManager : MonoBehaviour {
    private static WaitForSeconds _waitForSeconds0_02 = new(0.02f);
    private static WaitForSeconds _waitForSeconds2 = new(2f);
    private static WaitForSeconds _waitForSeconds4 = new(4f);
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private RectTransform titleUI;
    [SerializeField] private GameObject creditScreen;
    [SerializeField] private RectTransform creditUI;
    [SerializeField] private int slideSpeed;
    [SerializeField] private float uiSlideTarget;
    [SerializeField] private BackgroundAnimation bgAnimator;
    [SerializeField] private TMP_Text levelText;
    private bool isSliding = false;
    private bool isCredits = false;

    private readonly string[] levelStrings = {
        "greetings explorer, welcome to space! select a planet using your MOUSE and spin it with the A/S keys. If the planet lies on an orbital line, you can move it around using W/D. You can also use your arrow keys.",
        "the aim of our journey is to create as many places as we can to sustain life by rearranging the planets above. for example, that planet over there looks pretty hot, let's try blocking it from the sun.",
        "great job! there are other ways to change a planet's climate. this icy planet produces FROST, which can cool down any planets in their reach.",
        "experiment with different arrangements to discover new planets. press G to open the Gallery to keep track of the planets you've found.",
        "trigger chain reactions between planets by lining them up together. MOISTURE is another kind of planetary emission that can make planets more humid.",
        "HEAT and STEAM both make turn up their target planets' HEAT, but they also decrease HUMIDITY and increase ATMOSPHERIC PRESSURE respectively. Be sure to balance all three conditions to form life!",
        "that stormy planet is producing INWARD WINDS, which suck the atmosphere out of nearby planets.",
        "that serious-looking planet has a unique ability: it can push around any planets that aren't in orbit. pretty neat!",
        "if you've made a mistake or get stuck, press R to start from scratch.",
        "the logistical movement of the serious planets can be impacted by climate conditions. clear the way if you want to move some planets.",
        "the glass planet concentrates any heat it receives and pushes it out forward, increasing your planetary reach.",
        "remember, we can block the heat produced by the sun using other planets.",
        "there's nothing in the rule book about two planets facing each other...",
        "you're doing great, explorer. we're over halfway through our journey! things might start to get a bit more tricky...",
        "don't forget to check the Gallery to help you out on your journey!",
        "another new discovery! it appears this planet has a gravitational pull so strong, it can yank other planets like it's nothing!",
        "be careful to keep everything balanced here! don't forget you can reset with the R key.",
        "the universe really is a magnificent place, although it definitely can be a bit scary, we've seen frozen giants of gas and ice, flaming balls of plasma, and everything in between...",
        "... and as we've seen on this journey, the universe doesn't seem to take heed of how inhospitable it can be...",
        "...this is a scary thought to have, just how cold and indifferent the universe is. You nearly start to wonder- are we the only ones here?",
        "However, there is a certain beauty to the way the universe works, and we can see it in little place we like to call home!",
        "through no action of our own, our planet is positioned perfectly in this universe just where we need it, no clicking or arrow keys required. and it's beautiful...",
        "...in spite of the chaos and anarchy of the universe, we still found ourselves here. it truly is a gift.",
        "anyways- it looks like we're almost done, explorer! just one more stop and we've reached the end.",
    };

    void Awake() { GameManager.visualManager = this; }

    public void StartSliding() { 
        if (!isSliding) StartCoroutine(SlideMenu());
        levelText.text = "";    
    }

    public void CreditRoll() {
        isCredits = true;
        creditScreen.SetActive(true);
        creditUI.anchoredPosition = Vector2.left * uiSlideTarget;
        uiSlideTarget = 0f;
        if (!isSliding) StartCoroutine(SlideMenu());
    }

    public void SlideBackground() { bgAnimator.StartSliding(); }

    IEnumerator SlideMenu() {
        isSliding = true;
        RectTransform targetUI = isCredits ? creditUI : titleUI;
        while (targetUI.anchoredPosition.x > uiSlideTarget) {
            targetUI.anchoredPosition += slideSpeed  * Time.deltaTime * Vector2.left;
            yield return null;
        } 

        if (!isCredits) { titleScreen.SetActive(false); } 
        isSliding = false;
    }

    public IEnumerator LevelText(int level) {
        levelText.text = "";
        yield return _waitForSeconds2;
        string target = levelStrings[level];
        foreach (char letter in target) {
			levelText.text += letter;
			yield return _waitForSeconds0_02;
		} yield return _waitForSeconds4;
    }
}
