using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "Scriptable Objects/Planet")]
public class Planet : ScriptableObject {
    public bool isHome = false;
    
    [Header("Visuals")]
    public Sprite[] sprites;
    public int animationSpeed = 48;

    [Header("Emission Traits")]
    public bool hasEmissions;
    public int heat;
    public int humidity;
    public int atmosphere;
    public GameObject particlePrefab;

}