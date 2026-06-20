using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour {

    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;

    private int currentFrame;
    private int totalFrames;
    private int frameCount;
    [SerializeField] private int framesPerSprite = 48;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites != null) totalFrames = sprites.Length;
    }

    private void Update() {
        if (sprites == null || sprites.Length == 0) return;
        frameCount++;
        if (frameCount >= framesPerSprite) {
            frameCount = 0;
            currentFrame++;
            if (currentFrame >= totalFrames) currentFrame = 0;
            spriteRenderer.sprite = sprites[currentFrame];
        }
    }

    public void UpdateSpriteList(Sprite[] newSprites) {
        sprites = newSprites;
        totalFrames = newSprites.Length;
        currentFrame = 0;
        frameCount = 0;
    }
}