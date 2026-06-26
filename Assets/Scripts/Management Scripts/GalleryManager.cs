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
            { 0, new(galleryImages[0], "Mars", "Despite being our closet neighbour, the cold, dry, and airless red planet is vastly different to earth.", false) },
            { 1, new(galleryImages[1], "Frosted Chasm", "A cold, dry, ariy planet with jagged, ice-capped monuntains broken up by frosted chutes.", false) },
            { 2, new(galleryImages[2], "Saturn", "A cold, dry, gas giant, Saturn is known for its majestic rings of ice and rock.", false) },
            { 3, new(galleryImages[3], "Ice Cube", "With no atmosphere or heat, this planet has been encased in a block of ice.", false) },
            { 4, new(galleryImages[4], "Earth (Frozen)", "A planet not too dissimlar to our own, the low temperatures have covered this familar planet in sheets of ice.", false) },
            { 5, new(galleryImages[5], "Neptune", "Despite its smooth appearance, this shivering gas giant has no solid surface, being a composition of gas and liquids.", false) },
            { 6, new(galleryImages[6], "Haunted Planet", "With no atmosphere, this chilly wet planet only moves beneath the surface, given the illusion of a spirit trapped within.", false) },
            { 7, new(galleryImages[7], "Crystal Planet", "The conditions on this planet are ideal for the formation of beautiful vibrant crystals, which produce high amounts of moisture.", false) },
            { 8, new(galleryImages[8], "Uranus", "The smooth appearance of this ice giant hides its complex atmosphere, with several layers of clouds encompassing the planet.", false) },
            { 9, new(galleryImages[9], "Mercury", "Due to a fascinating combination of its thin atmosphere and its closeness to the sun, Mercury has a wide temperature range. ", false) },
            { 10, new(galleryImages[10], "Earth (Barren)", "A planet not too dissimlar to our own, the lack of water made it impossible for life to take form here.", false) },
            { 11, new(galleryImages[11], "Magnet", "With an enhanced gravitational pull, this celestial body pulls other planets in towards it.", false) },
            { 12, new(galleryImages[12], "Earth (Dull)", "A planet not too dissimilar to our own, the absence of water made it impossible for life to form here.", false) },
            { 13, new(galleryImages[13], "Home", "In a universe full of both empty space and anarchy, this simple mound of dirt found itself in the perfect conditions for us to call it home.", true) },
            { 14, new(galleryImages[14], "Earth (Toxic)", "A planet not too dissimilar to our own, the overproduction of pollutants enveloped this planet in a blanket of smog.", false) },
            { 15, new(galleryImages[15], "Serious", "A very serious planet that definitely isn't pandering to any Game Jam hosts or anything like that. Can push other planets with its very serious hat.", false) },
            { 16, new(galleryImages[16], "Earth (Submerged)", "A planet not too dissimilar to our own, an abundance of water covered all land beneath one giant ocean.", false) },
            { 17, new(galleryImages[17], "Storm", "A planet with winds so strong, they have been known to rip the atmosphere straight out from other planets.", false) },
            { 18, new(galleryImages[18], "Sand", "A sparse planet full of dunes and not much between them. The intense heat and lack of atmosphere make it especially hostile to most.", false) },
            { 19, new(galleryImages[19], "Factory", "It appears the hot, dry, airy climate of this planet was ideal for some far-away species to build a steam-producing forge on top of it.", false) },
            { 20, new(galleryImages[20], "Venus", "The other neighbour to our own planet, Venus is quite differnt to Mars, with a dense, hot atmosphere.", false) },
            { 21, new(galleryImages[21], "Radioactive", "Intense levels of heat unbound by the atmosphere have caused this planet to become strongly iradiated. don't stay for too long!", false) },
            { 22, new(galleryImages[22], "Earth (Boiling)", "A planet not too dissimilar to our own, overheating caused this planet to melt into itself, forming seas of molten earth.", false) },
            { 23, new(galleryImages[23], "Jupiter", "Jupiter is a planet of firsts- the first planet in the solar system to form, one of the first to be studied, and one of the first visible in the night sky.", false) },
            { 24, new(galleryImages[24], "Glass", "A unique combination of heat and pressure have resulted in an entire planet of glass! If this planet is heated further, it propgates the heat forward itself.", false) },
            { 25, new(galleryImages[25], "Spring", "The tropical appearance of this planet looks inviting, but with steamy hot springs coating its surface, some would prefer to observe from a distance.", false) },
            { 26, new(galleryImages[26], "Molten Planet", "A planet of pure molten magma, with some of the most extreme conditions in the universe, it produces intense heat that can warm other planets.", false) },
            { 27, new(galleryImages[27], "Asteroid", "These minor planets may not be forming life any time soon, but they're at least pretty to look at.", true) },
            { 28, new(galleryImages[28], "Sun", "The big, bright heart of our solar system, the Sun provides orbital stability and heat to all that surround it.", true) },
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
