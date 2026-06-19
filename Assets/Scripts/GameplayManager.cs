using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour {

    private GameObject selectedPlanet;
    private void Awake() { GameManager.gameplayManager = this; }

    private void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
                SelectPlanet(hit.collider.gameObject);
        }
    }

    private void FixedUpdate() {
        if (selectedPlanet != null)
            selectedPlanet.transform.Rotate(Vector3.forward, 20 * Time.fixedDeltaTime);
    }

    public void SelectPlanet(GameObject planet) => selectedPlanet = planet;
}