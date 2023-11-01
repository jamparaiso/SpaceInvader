using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] AudioSource shootSFX;
    public Projectile laserPrefab;
    public float speed = 5.0f;

    private bool _laserActive;

    private void Update()
    {
       Vector3 position = transform.position;

       if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
       {
         position.x -= speed * Time.deltaTime;
       }
       else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
       {
          position.x += speed * Time.deltaTime;
       }

        //get the edge of the screen
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        //clamp the the position of the player not to go out bounds
        position.x =Mathf.Clamp(position.x,leftEdge.x + 1.0f, rightEdge.x - 1.0f);
        transform.position = position;

       if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
       {
         Shoot();
       }
    }

    private void Shoot()
    {
        //this is to avoid rapid firing of laser
        //checks if the is a active laser projectile
        if(!_laserActive)
        {
            Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            //set a method that will trigger when the .destroyed event has been activated
            projectile.destroyed += LaserDestroyed;
            shootSFX.Play();
            _laserActive = true;
        }
    }

    private void LaserDestroyed()
    {
        _laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Invader") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            //when player is killed the should not restart
            GameManager.Instance.OnPlayerKilled(this);
        }

    }
}
