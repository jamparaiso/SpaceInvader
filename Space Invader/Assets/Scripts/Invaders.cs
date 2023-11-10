using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    [Header("Invaders")]
    public Invader[] prefabs;
    //speed is evaluated based on how many invaders has been killed
    public AnimationCurve speed;
    public Vector3 direction { get; private set; } = Vector3.right;
    public Vector3 initialPosition { get; private set; }

    [Header("Grid")]
    [SerializeField] int rows = 5;
    [SerializeField] int columns = 11;
    [SerializeField] float spacing = 2.0f;

    [Header("Missles")]
    [SerializeField] Projectile missilePrefab;
    [SerializeField] float missleAttackRate = 2.0f;

    private Vector3 _direction = Vector2.right;

    private void Awake()
    {
        initialPosition = transform.position;
        GenerateInvaderGrid();
    }

    private void GenerateInvaderGrid()
    {
        //generate the rows first
        for (int row = 0; row < this.rows; row++)
        {
            float width = spacing * (this.columns - 1);
            float height = spacing * (this.rows - 1);

            Vector2 centering = new Vector2(-width / 2, -height / 2);
            //position the sprite in the row / this uses row index of the loop
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * spacing), 0.0f);

            //then columns which instantiate a invader
            for (int col = 0; col < this.columns; col++)
            {
                //create a new invader and parent it to this transform
                Invader invader = Instantiate(this.prefabs[row], this.transform);

                //calculate and set the position of the invader
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
    {
        float percentKilled = CalculatePercentKilled();

        //speed is evaluated based on how many invaders has been killed
        this.transform.position += _direction * this.speed.Evaluate(percentKilled) * Time.deltaTime;

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
    private float CalculatePercentKilled()
    {
        float percentKilled;

        int totalCount = rows * columns;
        int amountAlive = GetAliveCount();
        int amountKilled = totalCount - amountAlive;
        percentKilled = (float)amountKilled / (float)totalCount;

        return percentKilled;
    }

    private void AdvanceRow()
    {
        //negates the position the invaders are moving
        _direction.x *= -1.0f;

        //moves the parent down a row
        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    public void ResetInvaders()
    {
        direction = Vector3.right;
        transform.position = initialPosition;

        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }
    }

    private void MissleAttack()
    {
        int aliveCount = GetAliveCount();

        //no missles should spawn if there are no invaders
        if (aliveCount == 0) 
        {
            return;
        }

        foreach(Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            //the lower the number of invaders left the higher the chance of launching missle attack
            if(Random.value < (1.0f / (float)aliveCount))
            {
                Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
                break; //break the loop so that only one missle attack can be launch
            }
        }
    }

    public int GetAliveCount()
    {
        int count = 0;
        foreach(Transform invader in this.transform)
        {
            if (invader.gameObject.activeSelf)
            {
                count++;
            }
        }
        return count;
    }
}
