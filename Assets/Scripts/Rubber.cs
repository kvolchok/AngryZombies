using UnityEngine;

public class Rubber : MonoBehaviour
{
    private LineRenderer _line;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
     }
    
    public void Stretch(Vector3 position)
    {
        _line.SetPosition(1, position);
    }
}