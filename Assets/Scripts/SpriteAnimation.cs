using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour {

    [SerializeField] private Sprite[] sprites;
    [SerializeField] int animationSpeed = 48;
    private SpriteRenderer spriteRenderer;

    private int currentFrame, totalFrames, frameCount;

    private void Awake() { 
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites != null) totalFrames = sprites.Length; 
    }

    private void Update() {
        if (sprites == null || sprites.Length <= 1) return;
        frameCount++;
        if (frameCount >= animationSpeed) {
            frameCount = 0;
            currentFrame++;
            if (currentFrame >= totalFrames) currentFrame = 0;

            Vector2 size = spriteRenderer.size;
            spriteRenderer.sprite = sprites[currentFrame];
            spriteRenderer.size = size;
        }
    }

    public void ChangeAnimation(Planet planet) {
        if (planet == null) {
            sprites = null;
            totalFrames = 0;
            frameCount = 0;
            currentFrame = 0;
            return;
        }

        sprites = planet.sprites;
        totalFrames = sprites != null ? sprites.Length : 0;
        animationSpeed = planet.animationSpeed;
        frameCount = 0;
        currentFrame = 0;
    }

    public void ResetAnimation() {
        frameCount = 0;
        currentFrame = 0;
    }
}