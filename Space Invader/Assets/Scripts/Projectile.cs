using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 1.0f;

    //native C# method
    //delegate pattern
    public System.Action destroyed; //callback

    private void Update()
    {
        //makes the projectile fly
        this.transform.position += this.direction * this.speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.destroyed != null)
        {
            //this method will trigger and delegate the event to other class that this is happening now
            this.destroyed.Invoke();
        }


            Destroy(this.gameObject);
    }
}
