using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EnemiesCounter : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _showWinScreen;
    
    [SerializeField]
    private TextMeshProUGUI _counterLabel;
    [SerializeField]
    private int _counter = 6;

    private void Awake()
    {
        _counterLabel.text = _counter.ToString();
    }

    [UsedImplicitly]
    public void AddScore()
    {
        _counter--;

        _counterLabel.text = _counter.ToString();

        if (_counter == 0)
        {
            _showWinScreen.Invoke();
        }
    }
}