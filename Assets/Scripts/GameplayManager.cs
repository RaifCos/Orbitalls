using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour {

    [Header("Player Inputs")]
    [SerializeField] private InputAction planetSpin;
    [SerializeField] private InputAction planetOrbit;

    [Header("Planet Speeds")]
    [SerializeField] private float planetSpinSpeed;
    [SerializeField] private float planetOrbitSpeed;

    [Header("Selected Planet")]
    private GameObject selectedPlanet;
    private GamePlanet selectedPlanetComponent;
    private int clickLayer;
    
    private void Awake() { 
        GameManager.gameplayManager = this;
        clickLayer = LayerMask.GetMask("Planet");    
    }

    private void OnEnable()  { planetSpin.Enable();  planetOrbit.Enable();  }
    private void OnDisable() { planetSpin.Disable(); planetOrbit.Disable(); }

    private void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, clickLayer);

            if (hit.collider != null) {
                selectedPlanet = hit.collider.gameObject;
                selectedPlanetComponent = selectedPlanet.GetComponent<GamePlanet>();
            }
        }

        if (selectedPlanet != null) {
            float spinInput = planetSpin.ReadValue<float>();
            
            if (spinInput != 0f) // Spin planet around its own axis.
                selectedPlanetComponent.ApplySpin(spinInput * planetSpinSpeed * Time.deltaTime);

            if (selectedPlanetComponent.OrbitsPlanet) { // Drive planet around the orbit of its parent planet.
                float orbitInput = planetOrbit.ReadValue<float>();
                if (orbitInput != 0f)
                    selectedPlanetComponent.DriveOrbit(orbitInput * planetOrbitSpeed, Time.deltaTime);
            }
        }
    }

    public void SelectPlanet(GameObject planet) => selectedPlanet = planet;
}