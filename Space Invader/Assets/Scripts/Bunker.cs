using UnityEngine;

public class Bunker : MonoBehaviour
{
    private void Awake()
    {
        ResetBunker();
    }

    public void ResetBunker()
    {
        gameObject.SetActive(true);
    }
}
