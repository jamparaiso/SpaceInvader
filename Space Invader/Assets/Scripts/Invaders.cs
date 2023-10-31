using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    [SerializeField] AudioSource invaderKilledSFX;
    public Invader[] prefabs;
    public Projectile missilePrefab;
    public int rows = 5;
    public int columns = 11;
    public float spacing = 2.0f;
    //speed is evaluated based on how many invaders has been killed
    public AnimationCurve speed;
    public float missleAttackRate = 2.0f;

    public int amountKilled {  get; private set; }
    public int amountAlive => this.totalInvaders - this.amountKilled;
    public int totalInvaders => this.rows * this.columns;
    public float percentKilled => (float)this.amountKilled / (float)this.totalInvaders;

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

                //call back function
               invader.killed += InvaderKilled;

               Vector3 position = rowPosition;
               position.x += col * spacing;
               //set to localPosition so that it wont affect the parent position
               invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissleAttack),this.missleAttackRate, this.missleAttackRate);
    }

    private void Update()
    {   //speed is evaluated based on how many invaders has been killed
        this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

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
    private void AdvanceRow()
    {
        //negates the position
        _direction.x *= -1.0f;

        Vector3 position = this.transform.position;
        //moves the parent one unit downward
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void InvaderKilled()
    {
        invaderKilledSFX.Play();
        this.amountKilled++;

        if (this.amountKilled > this.totalInvaders)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void MissleAttack()
    {
        foreach(Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            //the lower the number of invaders left the higher the chance of launching missle attack
            if(Random.value < (1.0f / (float)this.amountAlive))
            {
                Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
                break; //break the loop so that only one missle attack can be launch
            }
        }
    }
}
