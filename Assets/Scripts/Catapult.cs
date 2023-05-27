using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    [SerializeField]
    private GameObject _dotPrefab;
    [SerializeField]
    private int _trajectoryPointsCount = 20;

    [SerializeField]
    private float _speed = 8;
    [SerializeField]
    private float _time = 0.1f;
    
    [SerializeField]
    private Rigidbody2D _skull;

    [SerializeField]
    private AudioClip _throwSkull;

    private List<SpriteRenderer> _trajectoryPoints = new();

    private void Awake()
    {
        for (var i = 0; i < _trajectoryPointsCount; i++)
        {
            var dot = Instantiate(_dotPrefab, transform);
            var dotSprite = dot.GetComponent<SpriteRenderer>();
            dotSprite.enabled = false;
            _trajectoryPoints.Add(dotSprite);
        }
    }

    public void CalculateTrajectory(Vector3 startPoint)
    {
        var muzzleVelocity = GetForce(startPoint) / _skull.mass;
        var muzzleVelocityLength = muzzleVelocity.magnitude;
        
        var throwAngle = Mathf.Atan2(muzzleVelocity.y, muzzleVelocity.x) * Mathf.Rad2Deg;
        _skull.transform.eulerAngles = new Vector3(0, 0, throwAngle);

        for (var i = 1; i <= _trajectoryPoints.Count; i++)
        {
            var xCoordinates = muzzleVelocityLength * _time * i * Mathf.Cos(throwAngle * Mathf.Deg2Rad);
            var yCoordinates = muzzleVelocityLength * _time * i * Mathf.Sin(throwAngle * Mathf.Deg2Rad)
                               - Physics2D.gravity.magnitude * Mathf.Pow(_time * i, 2) / 2;

            var pointPosition = new Vector3(xCoordinates, yCoordinates, 1);
            _trajectoryPoints[i - 1].transform.position = startPoint + pointPosition;
            _trajectoryPoints[i - 1].enabled = true;
        }
    }

    public Vector2 GetForce(Vector3 startPoint)
    {
        var direction = transform.position - startPoint;
        var force = direction * _speed;
        return force;
    }

    public void ThrowSkull(Vector2 force)
    {
        AudioSource.PlayClipAtPoint(_throwSkull, transform.position);
        _skull.isKinematic = false;
        _skull.AddForce(force, ForceMode2D.Impulse);
    }

    public void HideTrajectory()
    {
        foreach (var trajectoryPoint in _trajectoryPoints)
        {
            trajectoryPoint.enabled = false;
        }
    }
}