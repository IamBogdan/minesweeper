using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private Sprite[] _digits;
    private SpriteRenderer _leftDigit, _midDigit, _rightDigit;
    private float _time = 0f;
    private bool _timerOn = false;

    void Awake() {
        _leftDigit = GameObject.Find("LeftDigit").GetComponent<SpriteRenderer>();
        _midDigit = GetComponent<SpriteRenderer>();
        _rightDigit = GameObject.Find("RightDigit").GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (_time < 1000 && _timerOn) {
            _time += Time.deltaTime;

            _rightDigit.sprite = _digits[(int)_time % 10];
            _midDigit.sprite = _digits[(int)_time / 10 % 10];
            _leftDigit.sprite = _digits[(int)_time / 100 % 10];
        }
    }

    public void StopTimer() {
        _timerOn = false;
    }

    public void StartTimer() {
        _timerOn = true;
    }

    public void ResetTimer() {
        _time = 0.0f;
    }
}
