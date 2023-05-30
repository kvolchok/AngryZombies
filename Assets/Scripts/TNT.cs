using UnityEngine;

public class TNT : MonoBehaviour
{
    [SerializeField] 
    public GameObject _explosionPrefab;
    [SerializeField] 
    public GameObject _explosionEffectPrefab;
    [SerializeField]
    private AudioSource _explosionSound;

    private ExplosionEffect _explosionEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(GlobalConstants.SKULL_TAG))
        {
            return;
        }

        CreateExplosion();
        PlayExplosionSound();
        _explosionEffect.Destroy();
        Destroy(gameObject);
    }
    
    private void PlayExplosionSound()
    {
        _explosionSound.Play();
    }
    
    private void CreateExplosion()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        var explosionEffect = Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
        _explosionEffect = explosionEffect.GetComponent<ExplosionEffect>();
    }
}