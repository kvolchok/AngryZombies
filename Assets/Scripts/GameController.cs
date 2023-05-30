using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D _touchArea;
    [SerializeField]
    private Rubber[] _rubbers;
    [SerializeField]
    private Catapult _catapult;
    [SerializeField]
    private Rigidbody2D _skull;
    
    [SerializeField]
    private float _minSkullVelocity = 0.1f;
    
    [SerializeField]
    private AudioClip _stretchingRubber;
    
    private Vector3 _center;
    private float _pullRadius;
    private Camera _camera;

    private bool _isButtonPressed;
    private bool _isSkullMoving;

    private void Awake()
    {
        _center = _touchArea.transform.position;
        _pullRadius = _touchArea.radius;
        _camera = Camera.main;
    }

    private void Update()
    {
        var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = _center.z;

        if (!IsMouseInTouchArea(mousePosition) && !_isButtonPressed)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !_isButtonPressed && !_isSkullMoving)
        {
            AudioSource.PlayClipAtPoint(_stretchingRubber, transform.position);
            _isButtonPressed = true;
        }
        
        if (Input.GetMouseButton(0) && !_isSkullMoving)
        {
            var direction = mousePosition - _center;
            direction = Vector2.ClampMagnitude(direction, _pullRadius);

            mousePosition = _center + direction;
            StretchRubber(mousePosition);
            _skull.position = mousePosition;
            _catapult.CalculateTrajectory(_skull.position);
        }

        if (Input.GetMouseButtonUp(0) && _isButtonPressed)
        {
            _isButtonPressed = false;
            
            ResetRubber();
            _catapult.HideTrajectory();
            var force = _catapult.GetForce(_skull.position);
            _catapult.ThrowSkull(force);
            _isSkullMoving = true;
        }

        if (_skull.velocity.magnitude <= _minSkullVelocity && _isSkullMoving)
        {
            ResetSkullPosition();
        }
    }

    public void ResetRubber()
    {
        foreach (var rubber in _rubbers)
        {
            rubber.gameObject.SetActive(false);
        }
    }
    
    public void ResetSkullPosition()
    {
        _isSkullMoving = false;
        
        _skull.freezeRotation = true;
        _skull.transform.rotation = Quaternion.identity;
        _skull.freezeRotation = false;
        _skull.velocity = Vector2.zero;
        _skull.transform.position = _center;
        
        _skull.isKinematic = true;
    }
    
    private bool IsMouseInTouchArea(Vector3 mousePosition)
    {
        return (_center - mousePosition).magnitude <= _pullRadius;
    }

    private void StretchRubber(Vector3 position)
    {
        foreach (var rubber in _rubbers)
        {
            rubber.gameObject.SetActive(true);
            rubber.Stretch(position);
        }
    }
}