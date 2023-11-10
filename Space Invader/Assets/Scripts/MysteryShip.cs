using UnityEngine;

public class MysteryShip : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float cycleTime = 30f;
    [SerializeField] AudioSource mysteryShipSFX;
    [SerializeField] Player player;
    public int score = 300;

    public Vector2 leftDestination {  get; private set; }
    public Vector2 rightDestination { get; private set; }
    public int direction { get; private set; } = -1; //initial value
    public bool spawned {  get; private set; }

    private void Start()
    {
        //get the screen coodinates so that we can set the destination
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        //offset the destination by 1 unit so that the ship go out of bounds
        leftDestination = new Vector2(leftEdge.x - 1f,transform.position.y);
        rightDestination = new Vector2(rightEdge.x + 1f,transform.position.y);

        Despawn();
    }

    private void Update()
    {
        //checks if the ship is spawned
        if (!spawned)
        {
            return;
        }

            if (direction == 1)
            {
                MoveRight();
            }
            else //direction is -1
            {
                MoveLeft();
            }
    }

    private void MoveRight()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        //ship reached the right edge of the screen
        if(transform.position.x >= rightDestination.x)
        {
            Despawn();
        }
    }

    private void MoveLeft()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        //ship reached the left edge of the screen
        if(transform.position.x <= leftDestination.x)
        {
            Despawn() ;
        }
    }

    private void Spawn()
    {
        spawned = true;
        mysteryShipSFX.Play();

        direction *= -1; //flips the direction everytime the ship reached the edge of the screen

        if (direction == 1)
        {
            transform.position = leftDestination;
        }
        else // direction -1
        {
            transform.position = rightDestination;
        }

    }

    private void Despawn()
    {
        mysteryShipSFX.Stop();
        spawned = false;

        if (direction == 1)
        {
            transform.position = rightDestination;
        }
        else //direction -1 also the initial position
        {
            transform.position = leftDestination;
        }

        Invoke(nameof(Spawn), cycleTime);
    }

    public void ResetPosition()
    {
        Despawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            Despawn();
            GameManager.Instance.OnMysteryShipKilled(this);
        }
    }

}
