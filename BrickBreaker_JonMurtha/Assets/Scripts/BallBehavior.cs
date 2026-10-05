using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    [Header("Ball Properties")]
    [SerializeField] private float _launchForce = 7.0f;
    [SerializeField, Range(0.0f, 1.0f)] private float _steepnessThreshold = 0.25f;
    [SerializeField, Range(0.0f, 1.0f)] private float _paddleInfluence = 0.4f;

    private Vector2 _direction;

    private AudioSource _source;
    [Header("audio Properties")]
    [SerializeField] private AudioClip _wallHit;
    [SerializeField] private AudioClip _paddleHit;
    [SerializeField] private AudioClip _fail;

    Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _source = GetComponent<AudioSource>();

        ResetBall();
    }

    private void Update()
    {
        _rb.simulated = GameBehavior.Instance.State == Utilities.GameState.Play;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            if (!Mathf.Approximately(collision.rigidbody.linearVelocityX, 0.0f))
            {
                Vector2 direction = _rb.linearVelocity * (1.0f - _paddleInfluence)
                                  + collision.rigidbody.linearVelocity * _paddleInfluence;
                direction.Normalize();
                CheckSteepness(ref direction);

                _rb.linearVelocity = _rb.linearVelocity.magnitude * direction;
            }
            _source.PlayOneShot(_paddleHit);
        }
        else
        {
            _source.pitch = Random.Range(0.9f, 1.1f);
            _source.volume = Random.Range(0.8f, 1.0f);
            _source.clip = _wallHit;
            _source.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameBehavior.Instance.ResetPrefab();
        _source.PlayOneShot(_fail);
        Invoke(nameof(DestroyBall), _fail.length);
        GameBehavior.Instance.ResetGame();
    }

    private void CheckSteepness(ref Vector2 direction)
    {
        if (Mathf.Abs(direction.y) < _steepnessThreshold)
        {
            direction.y += 0.5f * Mathf.Sign(direction.y);
            direction.Normalize();
        }
    }

    private void DestroyBall()
    {
        Destroy(gameObject);
    }

    private void ResetBall()
    {
        // _rb.linearVelocity = Vector2.zero;
        // transform.position = Vector3.zero;
        Vector2 direction = Random.insideUnitCircle.normalized;
        CheckSteepness(ref direction);
        direction.y = Mathf.Abs(direction.y);
        _rb.AddForce(direction * _launchForce, ForceMode2D.Impulse);
    }
}
