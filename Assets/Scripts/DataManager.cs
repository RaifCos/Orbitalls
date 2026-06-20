using UnityEngine;

public class DataManager : MonoBehaviour {

    [SerializeField] private Planet[] planets;

    private void Awake() => GameManager.dataManager = this;

    public Planet UpdatePlanetState(int heat, int humidity, int atmosphere) {
        foreach (Planet planet in planets) {
            if (planet.heat == heat && planet.humidity == humidity && planet.atmosphere == atmosphere) {
                return planet;
            }
        } return planets[0]; // Return Asteroid as a default. 
    }
}
