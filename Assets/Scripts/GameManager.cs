using System.Collections.Generic;
using UnityEngine;
struct PlanetData {
    public int index;
    public bool animated;
    public bool emissions;
    public GameObject prefab;

    public PlanetData(int i, bool a, bool e, GameObject prefab) {
        index = i;
        animated = a;
        emissions = e;
        this.prefab = prefab;
    }
}

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static GameplayManager gameplayManager;
    public static DataManager dataManager;
    public static PoolManager poolManager;

    [SerializeField] private GameObject[] planetObjects;

    Dictionary<(int, int, int), PlanetData> planetDict;

    void Awake() {
        instance = this;
        planetDict = new() {
            { (-1, -1, -1), new PlanetData(0,  false, false, planetObjects[0]) }, // Mars
            { (-1, -1,  0), new PlanetData(1,  false, false, planetObjects[1]) }, // Frozen Planet
            { (-1, -1,  1), new PlanetData(2,  false, false, planetObjects[2]) }, // Saturn
            { (-1,  0, -1), new PlanetData(3,  false, false, planetObjects[3]) }, // Ice Cube
            { (-1,  0,  0), new PlanetData(4,  false, false, planetObjects[4]) }, // Earth (Frozen)
            { (-1,  0,  1), new PlanetData(5,  false, false, planetObjects[5]) }, // Neptune
            { (-1,  1, -1), new PlanetData(6,  false, false, planetObjects[6]) }, // Haunted Planet
            { (-1,  1,  0), new PlanetData(7,  false, false, planetObjects[7]) }, // Crystal Planet
            { (-1,  1,  1), new PlanetData(8,  false, false, planetObjects[8]) }, // Uranus
            { ( 0, -1, -1), new PlanetData(9,  false, false, planetObjects[9]) }, // Mercury
            { ( 0, -1,  0), new PlanetData(10, false, false, planetObjects[10]) }, // Earth (Barren)
            { ( 0, -1,  1), new PlanetData(11, false, false, planetObjects[11]) }, // Magnet
            { ( 0,  0, -1), new PlanetData(12, false, false, planetObjects[12]) }, // Earth (Dull)
            { ( 0,  0,  0), new PlanetData(13, false, false, planetObjects[13]) }, // Earth
            { ( 0,  0,  1), new PlanetData(14, false, false, planetObjects[14]) }, // Earth (Toxic)
            { ( 0,  1, -1), new PlanetData(15, false, false, planetObjects[15]) }, // Serious
            { ( 0,  1,  0), new PlanetData(16, false, false, planetObjects[16]) }, // Earth (Submerged)
            { ( 0,  1,  1), new PlanetData(17, false, false, planetObjects[17]) }, // Storm
            { ( 1, -1, -1), new PlanetData(18, false, false, planetObjects[18]) }, // Sand
            { ( 1, -1,  0), new PlanetData(19, false, false, planetObjects[19]) }, // Factory
            { ( 1, -1,  1), new PlanetData(20, false, false, planetObjects[20]) }, // Venus
            { ( 1,  0, -1), new PlanetData(21, false, false, planetObjects[21]) }, // Radioactive
            { ( 1,  0,  0), new PlanetData(22, false, false, planetObjects[22]) }, // Earth (Overheated)
            { ( 1,  0,  1), new PlanetData(23, false, false, planetObjects[23]) }, // Jupiter
            { ( 1,  1, -1), new PlanetData(24, false, false, planetObjects[24]) }, // Glass
            { ( 1,  1,  0), new PlanetData(25, true, true, planetObjects[25]) }, // Spring
            { ( 1,  1,  1), new PlanetData(26, true, true, planetObjects[26]) }, // Molten Planet
        };
    }

    public int GetPlanetIndex(int x, int y, int z) => planetDict[(Mathf.Clamp(x, -1, 1), Mathf.Clamp(y, -1, 1), Mathf.Clamp(z, -1, 1))].index;

}
