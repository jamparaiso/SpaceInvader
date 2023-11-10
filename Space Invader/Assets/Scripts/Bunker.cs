using UnityEngine;

public class Bunker : MonoBehaviour
{
    public Texture2D splat;
    public Texture2D originalTexture {  get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public new BoxCollider2D collider { get; private set; }
    
    private void Awake()
    {
        //initialise the all the needed components
        //Take note!!!!!
        //please initialise the Read/Write to TRUE of Bunker and the Splat in Assets folder
        //this is needed to manipulate the sprite during runtime
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        originalTexture = spriteRenderer.sprite.texture;

        ResetBunker();
    }

    public void ResetBunker()
    {
        //each of the bunker needs a unique instance of the sprite texture
        //since it will be modified at source
        CopyTexture(originalTexture);

        gameObject.SetActive(true);
    }

    private void CopyTexture(Texture2D source)
    {
        //copies the original texture of the bunker during runtime so that it could be loaded again.
        Texture2D copy = new Texture2D(source.width, source.height, source.format, false);
        copy.filterMode = source.filterMode;
        copy.anisoLevel = source.anisoLevel;
        copy.wrapMode = source.wrapMode;
        copy.SetPixels(source.GetPixels());
        copy.Apply();

        Sprite sprite = Sprite.Create(copy, spriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f), spriteRenderer.sprite.pixelsPerUnit);
        spriteRenderer.sprite = sprite;
    }

    public bool CheckCollision(BoxCollider2D boxCollider, Vector3 hitpoint)
    {
        Vector2 offset = boxCollider.size / 2;

        //checks where the bunker was hit by the projectile and apply a splat to it
        //to make it looks like it was destroyed
        return Splat(hitpoint) ||
               Splat(hitpoint + (Vector3.down * offset.y)) ||
               Splat(hitpoint + (Vector3.up * offset.y)) ||
               Splat(hitpoint + (Vector3.left * offset.x)) ||
               Splat(hitpoint + (Vector3.right * offset.x));
    }

    private bool Splat(Vector3 hitpoint)
    {
        int px;
        int py;
        
        //checks if the hitpoint is empty pixel or not
        //if empty lets the bullet go thru else apply a splat to it
        if(!CheckPoint(hitpoint,out px,out py))
        {
            return false;
        }

        Texture2D texture = spriteRenderer.sprite.texture;

        //makes the splat centered in the hit point
        px -= splat.width / 2;
        py -= splat.height / 2;

        int startX = px;

        for (int y = 0; y < splat.height; y++)
        {
            px = startX;

            for (int x = 0; x < splat.width; x++)
            {
                Color pixel = texture.GetPixel(px,py);
                pixel.a *= splat.GetPixel(x, y).a;
                texture.SetPixel(px,py,pixel);
                px++;
            }
            py++;
        }
        texture.Apply();
        return true;
    }

    private bool CheckPoint(Vector3 hitpoint, out int px, out int py)
    {
        Vector3 localPoint = transform.InverseTransformPoint(hitpoint);

        localPoint.x += collider.size.x / 2;
        localPoint.y += collider.size.y / 2;

        Texture2D texture = spriteRenderer.sprite.texture;

        px = (int)((localPoint.x / collider.size.x) * texture.width);
        py = (int)((localPoint.y / collider.size.y) * texture.height);

        return texture.GetPixel(px,py).a != 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            gameObject.SetActive(false);
        }

    }
}
