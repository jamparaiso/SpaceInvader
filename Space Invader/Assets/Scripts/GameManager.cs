using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //singleton pattern
    public static GameManager Instance {  get; private set; }

    [SerializeField] AudioSource playerKilledSFX;
    [SerializeField] AudioSource invaderKilledSFX;
    [SerializeField] Text scoreText;
    [SerializeField] Text livesText;
    [SerializeField] Text gameOverText;
    [SerializeField] Text tryAgainText;


    private Player player;
    private Invaders invaders;
    private MysteryShip mysteryShip;
    private Bunker[] bunkers;

    public int score {  get; private set; }
    public int lives { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
        bunkers = FindObjectsOfType<Bunker>();

        NewGame();

    }

    private void Update()
    {
       if (lives <= 0 && Input.GetKeyDown(KeyCode.Return))
        {
           NewGame();
         }

    }

    private void NewGame()
    {
        gameOverText.gameObject.SetActive(false);
        tryAgainText.gameObject.SetActive(false);

        SetLives(3);
        SetScore(0);
        NewRound();


    }

    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

        mysteryShip.gameObject.SetActive(true);
        mysteryShip.ResetPosition();

        for (int i = 0; i < bunkers.Length; i++)
        {
            Debug.Log(bunkers.Length);
            bunkers[i].ResetBunker();
        }


        Respawn();
    }

    private void Respawn()
    {
        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        invaders.gameObject.SetActive(false);
        mysteryShip.gameObject.SetActive(false);

        gameOverText.gameObject.SetActive(true);
        tryAgainText.gameObject.SetActive(true);
    }

    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives,0);
        livesText.text = lives.ToString();
    }

    public void OnPlayerKilled(Player player)
    {
        playerKilledSFX.Play();
        SetLives(lives - 1);
        player.gameObject.SetActive(false);

        if(lives > 0)
        {
            Invoke(nameof(NewRound), 1f);
        }else
        {
            GameOver();
        }
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }


    public void OnInvaderKilled(Invader invader) 
    {
        invaderKilledSFX.Play();
        invader.gameObject.SetActive(false);

        SetScore(score + invader.score);

        if(invaders.GetAliveCount() == 0)
        {
            NewRound();
        }
    }
    public void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        SetScore(score + mysteryShip.score);
    }

    public void OnBoundaryReached()
    {
        if (invaders.gameObject.activeSelf)
        {
            invaders.gameObject.SetActive(false);
            OnPlayerKilled(player);
        }
    }
}
