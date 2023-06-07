using UnityEngine;
using UnityEngine.Events;

public class Skull : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _outOfMapEvent;
    
    [SerializeField]
    private AudioClip _hittingStone;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalConstants.STONE_TAG))
        {
            AudioSource.PlayClipAtPoint(_hittingStone, transform.position);
        }

        if (collision.gameObject.CompareTag(GlobalConstants.OUT_OF_MAP_TAG))
        {
            _outOfMapEvent.Invoke();
        }
    }
}