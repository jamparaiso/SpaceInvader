using UnityEngine;

//attach this script to laser and missile sprite
public class Projectile : MonoBehaviour
{
    public new BoxCollider2D collider {  get; private set; }
    public Vector3 direction = Vector3.up;
    [SerializeField] float speed = 1.0f;


    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //makes the projectile fly
        //you need to set the direction in editor 1 makes it fly upwards / 0 makes it fly downwards
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (this.destroyed != null)
        //{
        //    //this method will trigger and delegate the event to other class that this is happening now
        //    this.destroyed.Invoke();
        //}
        
        //Destroy(this.gameObject);
        CheckCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    private void CheckCollision(Collider2D collision)
    {
        //initialise which bunker the projectile has collided with
        Bunker bunker = collision.gameObject.GetComponent<Bunker>();

        //checks if it collides with something
        if(bunker == null || bunker.CheckCollision(collider, transform.position))
        {
            Destroy(gameObject);
        }

    }
}
