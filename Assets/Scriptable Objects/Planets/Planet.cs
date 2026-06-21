using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "Scriptable Objects/Planet")]
public class Planet : ScriptableObject {
    [Header("Identification Info")]
    public string internalName;
    public string externalName;
    [TextArea(3, 6)]
    public string description;
    [Header("Animation")]
    public Sprite[] sprites;
    public int animationSpeed = 48;

    [Header("Planet Traits")]
    public int heat;
    public int humidity;
    public int atmosphere;
    public bool changesState = true;

    [Header("Elemental Properties")]
    public Element element;
}
