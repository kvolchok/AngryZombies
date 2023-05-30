using System.Collections;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField]
    private float _destructionDelay = 1f;

    public void Destroy()
    {
        StartCoroutine(DestroyExplosion());
    }
    
    private IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(_destructionDelay);
        
        Destroy(gameObject);
    }
}