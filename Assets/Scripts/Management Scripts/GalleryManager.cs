using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

struct GalleryIcon {
    public GameObject obj;
    public string name;
    public string description;
    public bool unlocked;

    public GalleryIcon(GameObject i, string n, string d, bool u) {
        obj = i;
        name = n;
        description = d;
        unlocked = u;
    }
}

public class GalleryManager : MonoBehaviour {
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Image image;
    [SerializeField] private Sprite hiddenSprite;

    [SerializeField] private GameObject[] galleryImages; 
    Dictionary<int, GalleryIcon> iconSet; 

    void Awake() {
        GameManager.galleryManager = this;
        iconSet = new() {
            { 0, new(galleryImages[0], "Mars", "Mars Description", false) },
            { 1, new(galleryImages[1], "Frozen Planet", "Frozen Planet Description", false) },
            { 2, new(galleryImages[2], "Saturn", "Saturn Description", false) },
            { 3, new(galleryImages[3], "Ice Cube", "Ice Cube Description", false) },
            { 4, new(galleryImages[4], "Earth (Frozen)", "Earth (Frozen) Description", false) },
            { 5, new(galleryImages[5], "Neptune", "Neptune Description", false) },
            { 6, new(galleryImages[6], "Haunted Planet", "Haunted Planet Description", false) },
            { 7, new(galleryImages[7], "Crystal Planet", "Crystal Planet Description", false) },
            { 8, new(galleryImages[8], "Uranus", "Uranus Description", false) },
            { 9, new(galleryImages[9], "Mercury", "Mercury Description", false) },
            { 10, new(galleryImages[10], "Earth (Barren)", "Earth (Barren) Description", false) },
            { 11, new(galleryImages[11], "Magnet", "Magnet Description", false) },
            { 12, new(galleryImages[12], "Earth (Dull)", "Earth (Dull) Description", false) },
            { 13, new(galleryImages[13], "Earth", "Earth Description", true) },
            { 14, new(galleryImages[14], "Earth (Toxic)", "Earth (Toxic) Description", false) },
            { 15, new(galleryImages[15], "Seri1ous", "Seri1ous Description", false) },
            { 16, new(galleryImages[16], "Earth (Submerged)", "Earth (Submerged) Description", false) },
            { 17, new(galleryImages[17], "Storm", "Storm Description", false) },
            { 18, new(galleryImages[18], "Sand", "Sand Description", false) },
            { 19, new(galleryImages[19], "Factory", "Factory Description", false) },
            { 20, new(galleryImages[20], "Venus", "Venus Description", false) },
            { 21, new(galleryImages[21], "Radioactive", "Radioactive Description", false) },
            { 22, new(galleryImages[22], "Earth (Overheated)", "Earth (Overheated) Description", false) },
            { 23, new(galleryImages[23], "Jupiter", "Jupiter Description", false) },
            { 24, new(galleryImages[24], "Glass", "Glass Description", false) },
            { 25, new(galleryImages[25], "Spring", "Spring Description", false) },
            { 26, new(galleryImages[26], "Molten Planet", "Molten Planet Description", false) },
            { 27, new(galleryImages[27], "Asteroid", "Asteroid Description", true) },
            { 28, new(galleryImages[28], "Sun", "Sun Description", true) },
        };
    }

    public void UnlockEntry(int entry) {
        GalleryIcon g = iconSet[entry];
        if (g.unlocked) return;
        g.unlocked = true;
        g.obj.GetComponent<Image>().color = Color.white;
        iconSet[entry] = g; 
    }

    public void DisplayEntry(int entry) {
        if(entry == -1) {
            title.text = "";
            description.text = "";
            image.sprite = hiddenSprite;
            return;
        } 
        
        GalleryIcon g = iconSet[entry];
        if (g.unlocked) {
            title.text = g.name;
            description.text = g.description;
            image.sprite = g.obj.GetComponent<Image>().sprite;
        } else {
            title.text = "???";
            description.text = "The stars have not yet been aligned to reveal this planet...";
            image.sprite = hiddenSprite;
        }
    }
}
