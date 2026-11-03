using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour {

    [Header("Player Inputs")]
    [SerializeField] private InputAction planetSpin;
    [SerializeField] private InputAction planetOrbit;
    [SerializeField] private InputAction resetAction;
    [SerializeField] private InputAction galleryAction;
    [SerializeField] private InputAction levelSelectAction;

    [Header("Planet Speeds")]
    [SerializeField] private float planetSpinSpeed;
    [SerializeField] private float planetOrbitSpeed;

    [Header("Selected Planet")]
    private GameObject selectedPlanet;
    private GamePlanet selectedPlanetComponent;
    private int clickLayer;
    private int currentMenu;
    [SerializeField] private GameObject selectedLight;

    [Header("Gallery Stuff")]
    [SerializeField] private GameObject galleryCanvas;
    [SerializeField] private GameObject selectCanvas;
    private bool galleryOpen = false;
    private bool levelSelectOpen = false;
    
    private void Awake() { 
        GameManager.gameplayManager = this;
        clickLayer = LayerMask.GetMask("Planet");    
    }

    private void OnEnable()  { planetSpin.Enable();  planetOrbit.Enable(); resetAction.Enable(); galleryAction.Enable(); levelSelectAction.Enable(); }
    private void OnDisable() { planetSpin.Disable(); planetOrbit.Disable(); resetAction.Disable(); galleryAction.Disable(); levelSelectAction.Disable(); }

    private void Update() {
        if (!GameManager.instance.isPlaying) { return; }

        // Check to see if menus should be Opened/Closed.
        if (galleryAction.WasPressedThisFrame()) AlternateMenus(currentMenu == 0? 1 : 0);
        if (levelSelectAction.WasPressedThisFrame()) AlternateMenus(currentMenu == 0? 2 : 0); 
        if (galleryOpen || levelSelectOpen) return;

        if (resetAction.WasPressedThisFrame()) { GameManager.instance.ResetLevel(); }

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, clickLayer);

            if (hit.collider != null) {
                selectedPlanet = hit.collider.gameObject;
                selectedPlanetComponent = selectedPlanet.GetComponent<GamePlanet>();
            }
        }

        if (selectedPlanetComponent != null) {
            float spinInput = planetSpin.ReadValue<float>();
            
            if (spinInput != 0f) // Spin planet around its own axis.
                selectedPlanetComponent.ApplySpin(spinInput * planetSpinSpeed * Time.deltaTime);

            if (selectedPlanetComponent.OrbitsPlanet) { // Drive planet around the orbit of its parent planet.
                float orbitInput = planetOrbit.ReadValue<float>();
                if (orbitInput != 0f)
                    selectedPlanetComponent.DriveOrbit(orbitInput * planetOrbitSpeed, Time.deltaTime);
            }

            selectedLight.transform.position = selectedPlanet.transform.position;
        }
    }

    private void LateUpdate() { GamePlanet.ResolveDirtyPlanets(); }

    public void AlternateMenus(int menu) {
        currentMenu = menu;
        switch(menu) {
          case 0: // Return to Game
            galleryOpen = false;
            levelSelectOpen = false;
            break;
          case 1: // Gallery
            galleryOpen = true;
            levelSelectOpen = false;
            break;
          case 2: // Level Select
            galleryOpen = false;
            levelSelectOpen = true;
            break;  
        }
        galleryCanvas.SetActive(galleryOpen); 
        selectCanvas.SetActive(levelSelectOpen);
    }

    public void SelectPlanet(GameObject planet) => selectedPlanet = planet;

    public void ResetSelection() {
        selectedPlanet = null;
        selectedPlanetComponent = null;
        selectedLight.transform.position = Vector3.up * 50f;
    }
}