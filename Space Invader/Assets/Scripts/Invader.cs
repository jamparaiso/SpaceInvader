using UnityEngine;

public class Invader : MonoBehaviour
{
    //array of sprites to animate
    public Sprite[] animationSprites;
    //how often in cycles to next sprite
    public float animationTime = 1.0f;

    //access to which sprite wants to render
    private SpriteRenderer _spriteRenderer;
    //keep track which sprite in use / index of the animationSprites
    private int _animationFrame;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    private void AnimateSprite()
    {
        _animationFrame++;

        //make sure that index wont exceed the number of sprites
        if (_animationFrame >= animationSprites.Length)
        {
            _animationFrame = 0;
        }
        //animate the sprite
        _spriteRenderer.sprite = this.animationSprites[_animationFrame];
    }
}
