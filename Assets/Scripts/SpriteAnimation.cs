using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour {

    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer sr => spriteRenderer ??= GetComponent<SpriteRenderer>();

    private int currentFrame, totalFrames, frameCount;
    private int animationSpeed = 48;

    private void Awake() { if (sprites != null) totalFrames = sprites.Length; }

    private void Update() {
        if (sprites == null || sprites.Length == 0) return;
        frameCount++;
        if (frameCount >= animationSpeed) {
            frameCount = 0;
            currentFrame++;
            if (currentFrame >= totalFrames) currentFrame = 0;

            Vector2 size = sr.size;
            sr.sprite = sprites[currentFrame];
            sr.size = size;
        }
    }

    public void UpdateSpriteList(Sprite[] newSprites, int aS) {
        sprites = newSprites;
        totalFrames = newSprites.Length;
        currentFrame = 0;
        frameCount = 0;
        animationSpeed = aS;

        Vector2 size = sr.size;
        sr.sprite = sprites[0];
        sr.size = size;
    }
}