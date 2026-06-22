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
        if (sprites == null || sprites.Length == 0) return;
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
}