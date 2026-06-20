using UnityEngine;

public class DataManager : MonoBehaviour {

    [SerializeField] private Planet[] planets;

    private void Awake() => GameManager.dataManager = this;

    public Planet UpdatePlanetState(int heat, int humidity, int atmosphere) {
        foreach (Planet planet in planets) {
            // Cap planet traits to -1 to 1 range for comparison.
            int cHe = heat < -1 ? -1 : heat > 1 ? 1 : heat;
            int cHu = humidity < -1 ? -1 : humidity > 1 ? 1 : humidity;
            int cA = atmosphere < -1 ? -1 : atmosphere > 1 ? 1 : atmosphere;

            // Transform the planet into a new type based on its traits.
            if (planet.heat == cHe && planet.humidity == cHu && planet.atmosphere == cA) { return planet; }
        } return planets[0]; // Return Asteroid as a default. 
    }
}
