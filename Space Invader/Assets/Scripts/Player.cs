using UnityEngine;

public class Player : MonoBehaviour
{
    public Projectile laserPrefab;
    public float speed = 5.0f;

    private bool _laserActive;

    private void Update()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position += Vector3.left * this.speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += Vector3.right * this.speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
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
            
            _laserActive = true;
        }

    }

    private void LaserDestroyed()
    {
        _laserActive = false;
    }
}
