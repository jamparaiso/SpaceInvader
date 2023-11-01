using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton pattern
    public static GameManager Instance {  get; private set; }

    [SerializeField] AudioSource deathSFX;

    private Player player;
    private Invaders invaders;
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
        bunkers = FindObjectsOfType<Bunker>();

        SetLives(3);
    }

    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

        for (int i = 0; i < bunkers.Length; i++)
        {
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
        //game over
    }

    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives,0);
    }

    public void OnPlayerKilled(Player player)
    {
        deathSFX.Play();

        //when player is killed the should not restart
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
}
