using UnityEngine;
using TMPro;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;

    private AudioSource _source;
    [SerializeField] private AudioClip _brickHit;

    [SerializeField] private TMP_Text _scoreUI;
    private int _score;

    // Public variable
    public int Score
    {
        // get
        // {
        //     return _score * 10;
        // }

        get => _score;

        set
        {
            _score = value;
            // _scoreUI.text = Score.ToString();
            _scoreUI.SetText(Score.ToString());
        }

        // set => _score = value;
    }

    void Awake()
    {
        // Singleton pattern

        if (Instance == null)
        {
            Instance = this;
            Debug.Log("New instance initialized...");
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Duplicate instance found and deleted...");
        }

        // Alternate Initialization
        // if (Instance != null && Instance != this)
        // {
        //    Destroy(this);
        // }
        // else 
        // {
        //    Instance = this;
        // }
    }

    void Start()
    {
        ResetGame();
        _source = GetComponent<AudioSource>();
    }

    void ResetGame()
    {
        Score = 0;
    }

    public void ScorePoint()
    {
        Score++;
        _source.PlayOneShot(_brickHit);
    }
}
