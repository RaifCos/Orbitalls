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
    
    private void Awake() { GameManager.gameplayManager = this; }

    private void OnEnable()  { planetSpin.Enable();  planetOrbit.Enable();  }
    private void OnDisable() { planetSpin.Disable(); planetOrbit.Disable(); }

    private void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Planet")) {
                selectedPlanet = hit.collider.gameObject;
                selectedPlanetComponent = selectedPlanet.GetComponent<GamePlanet>();
            }
        }

        if (selectedPlanet != null) {
            // Spin Planet around its own axis.
            float spinInput = planetSpin.ReadValue<float>();
            selectedPlanet.transform.Rotate(Vector3.forward, spinInput * planetSpinSpeed * Time.deltaTime);
            
            // Orbit Planet around its parent (if applicable).
            if (selectedPlanetComponent.OrbitsPlanet) {
                float orbitInput = planetOrbit.ReadValue<float>();
                selectedPlanetComponent.DriveOrbit(orbitInput * planetOrbitSpeed, Time.deltaTime); // Only drives angle
            }
        }
    }

    public void SelectPlanet(GameObject planet) => selectedPlanet = planet;
}