using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "Scriptable Objects/Element")]
public class Element : ScriptableObject {
    [Header("Identification Info")]
    public string internalName;
    public string externalName;
    [TextArea(3, 6)]
    public string description;

    [Header("Emissions and Abilites")]
    public int heatEffect;
    public int humidityEffect;
    public int atmosphereEffect;

}
