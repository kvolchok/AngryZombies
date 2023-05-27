using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _enemyDied;
    
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField] 
    private AudioClip _deathSound;

    [SerializeField]
    private float _maxFallingVelocity = 0.5f;
    [SerializeField]
    private float _maxRotationAngle = 30f;

    [SerializeField]
    private AudioClip _zombieSound;
    [SerializeField]
    private float _zombieSoundDelay = 3f;

    private float _currentTime;
    
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime > _zombieSoundDelay)
        {
            PlayZombieSound();
            _currentTime = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalConstants.SKULL_TAG))
        {
            Die();
            return;
        }

        var collisionRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (IsFellDown(collisionRigidbody))
        {
            Die();
            return;
        }
        
        if (HasTurned())
        {
            Die();
        }
    }

    private bool IsFellDown(Rigidbody2D rigidbody)
    {
        return rigidbody != null && rigidbody.velocity.magnitude >= _maxFallingVelocity;
    }
    
    private bool HasTurned()
    {
        var rotation = _rigidbody.rotation;
        return rotation <= -_maxRotationAngle || rotation >= _maxRotationAngle;
    }
    
    private void Die()
    {
        _enemyDied.Invoke();
        
        // Создаем эффект "взрыв" на месте убитого зомби.
        CreateExplosion();
        // ПРоигрываем звук смерти зомби.
        PlayDeathSound();
        // Разрушаем объект зомби.
        Destroy(gameObject);
    }

    private void PlayZombieSound()
    {
        AudioSource.PlayClipAtPoint(_zombieSound, transform.position);
    }
    
    private void PlayDeathSound()
    {
        AudioSource.PlayClipAtPoint(_deathSound, transform.position);
    }
    
    private void CreateExplosion()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
    }
}