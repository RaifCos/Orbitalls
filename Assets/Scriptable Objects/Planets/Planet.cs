using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "Scriptable Objects/Planet")]
public class Planet : ScriptableObject {
    [Header("Identification Info")]
    public string internalName;
    public string externalName;
    [TextArea(3, 6)]
    public string description;
    public Sprite sprite;

    [Header("Planet Traits")]
    public int heat;
    public int humidity;
    public int atmosphere;

    [Header("Elemental Properties")]
    public Element element;
    public float reach; 
}
