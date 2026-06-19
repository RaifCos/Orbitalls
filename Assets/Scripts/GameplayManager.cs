using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour {

    private GameObject selectedPlanet;
    [SerializeField] private InputAction planetSpin;
    [SerializeField] private float planetSpinSpeed;
    
    private void Awake() { GameManager.gameplayManager = this; }

    private void OnEnable() => planetSpin.Enable();
    private void OnDisable() => planetSpin.Disable();

    private void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null) selectedPlanet = hit.collider.gameObject;
        }

        if (selectedPlanet != null) {
            float spinInput = planetSpin.ReadValue<float>();
            selectedPlanet.transform.Rotate(Vector3.forward, spinInput * planetSpinSpeed * Time.deltaTime);
        }
    }

    public void SelectPlanet(GameObject planet) => selectedPlanet = planet;
}