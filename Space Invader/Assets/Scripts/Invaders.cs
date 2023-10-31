using UnityEngine;

public class Invaders : MonoBehaviour
{
    public Invader[] prefabs;
    public int rows = 5;
    public int columns = 11;
    public float spacing = 2.0f;
    public float speed = 1.0f;

    private Vector3 _direction = Vector2.right;

    private void Awake()
    {
        //generate the rows first
        for (int row = 0; row < this.rows; row++)
        {

            float width = spacing * (this.columns - 1);
            float height = spacing * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2,-height / 2);
            //position the sprite in the row / this uses row index of the loop
            Vector3 rowPosition = new Vector3(centering.x,centering.y + (row * spacing) , 0.0f);

            //then columns which instantiate a invader
            for(int col = 0; col < this.columns; col++)
            {
                //corresponds to the array declared above
               Invader invader = Instantiate(this.prefabs[row], this.transform);
                Vector3 position = rowPosition;
                position.x += col * spacing;
                //set to localPosition so that it wont affect the parent position
                invader.transform.localPosition = position;
            }
        }
    }

    private void Update()
    {
            this.transform.position += _direction * this.speed * Time.deltaTime;

            //get the edge of the screen
            Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
            Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

            //iterate all child in the parent
            foreach (Transform invader in this.transform)
            {
                //checks if the child is active in the parent
                //a child can be disabled when player shot it
                if (!invader.gameObject.activeInHierarchy)
                {
                    continue;
                }

                //checks if a invader touches the edge of the screen
                //also adds a padding so that it wont go off screen
                if (_direction == Vector3.right && invader.position.x > (rightEdge.x - 1.0f))
                {
                    AdvanceRow();
                }
                else if (_direction == Vector3.left && invader.position.x < (leftEdge.x + 1.0f))
                {
                    AdvanceRow();
                }
            }
    }

    private void MoveInvader() 
    {
    
    }

    private void AdvanceRow()
    {
        //negates the position
        _direction.x *= -1.0f;

        Vector3 position = this.transform.position;
        //moves the parent one unit downward
        position.y -= 1.0f;
        this.transform.position = position;
    }
}
