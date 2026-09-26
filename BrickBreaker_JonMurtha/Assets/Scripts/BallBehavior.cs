using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    [SerializeField] private float _launchForce = 5.0f;
    [SerializeField] private float _paddleInfluence = 0.4f;

    private Vector2 direction;

    Rigidbody2D _rb;

    private AudioSource _source;
    [SerializeField] private AudioClip _wallHit;
    [SerializeField] private AudioClip _paddleHit;
    [SerializeField] private AudioClip _fail;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _source = GetComponent<AudioSource>();

        ResetBall();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            if (!Mathf.Approximately(collision.rigidbody.linearVelocityX, 0.0f))
            {
                Vector2 direction = _rb.linearVelocity * (1.0f - _paddleInfluence)
                                  + collision.rigidbody.linearVelocity * _paddleInfluence;
                _rb.linearVelocity = _rb.linearVelocity.magnitude * direction.normalized;
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
        ResetBall();
        _source.PlayOneShot(_fail);
    }

    private void ResetBall()
    {
        _rb.linearVelocity = Vector2.zero;
        transform.position = Vector3.zero;
        Vector2 direction = Random.insideUnitCircle.normalized;
        _rb.AddForce(direction * _launchForce, ForceMode2D.Impulse);
    }
}
