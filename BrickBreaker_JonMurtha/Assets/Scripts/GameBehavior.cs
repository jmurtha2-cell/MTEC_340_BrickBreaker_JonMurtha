using UnityEngine;
using TMPro;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;

    private Utilities.GameState _state;

    public Utilities.GameState State
    {
        get => _state;

        set
        {
            _state = value;
            _pauseUI.enabled = State == Utilities.GameState.Pause;
        }
    }
    [SerializeField] private TMP_Text _pauseUI;

    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Transform _ballParent;
    [SerializeField] private GameObject _brickPrefab;
    [SerializeField] private Transform _brickParent;

    private GameObject _ball;
    private GameObject _brick;

    private AudioSource _source;
    [SerializeField] private AudioClip _brickHit;

    [SerializeField] private Transform[] Spawnpoints;

    private float checkRadius = 0.1f;
    [SerializeField] private LayerMask objectLayer;

    //Player Score Utility
    // ----------------------------------------
    [SerializeField] private TMP_Text _scoreUI;
    [SerializeField] private int _numBricks = 15;
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
    // ----------------------------------------

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
        ResetPrefab();
        _source = GetComponent<AudioSource>();

        State = Utilities.GameState.Play;
        //_ball = Instantiate(_ballPrefab, Vector3.zero, Quaternion.identity, _ballParent);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            State = State == Utilities.GameState.Play ? Utilities.GameState.Pause : Utilities.GameState.Play;
        }
        if (!_ball)
        {
            ResetPrefab();
        }
    }

    public void ScorePoint()
    {
        Score++;
        _source.PlayOneShot(_brickHit);
        if (Score >= _numBricks)
        {
            ResetGame();
            Destroy(_ball);
        }
    }

    public void ResetGame()
    {
        Score = 0;

        Invoke("fixBricks", 0.1f);
    }

    
    public void ResetPrefab()
    {
        _ball = Instantiate(_ballPrefab, _ballParent.transform.position, Quaternion.identity, _ballParent);
    }

    private bool ObjectIsThere(Transform location)
    {
        Collider2D hit = Physics2D.OverlapCircle(location.position, checkRadius, objectLayer);
        bool objectThere = (hit != null);
        Debug.Log(objectThere);
        return objectThere;
    }

    private void fixBricks()
    {
        foreach (Transform location in Spawnpoints)
        {
            if (!ObjectIsThere(location))
            {
                _brick = Instantiate(_brickPrefab, location.position, Quaternion.identity, _brickParent);
            }
        }
    }
}

