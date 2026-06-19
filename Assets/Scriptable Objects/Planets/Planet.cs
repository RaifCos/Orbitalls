using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "Scriptable Objects/Planet")]
public class Planet : ScriptableObject {
    [Header("Identification Info")]
    public string internalName;
    public string externalName;
    [TextArea(3, 6)]
    public string description;
    public Sprite sprite;

    [Header("Planet Properties")]
    public Element elementType;
    public int outputDirection; // 0 = None, 1 = Ray, 2 = Radial
    public float radius; 
}
