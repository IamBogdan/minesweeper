using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private Sprite[] _digits;
    private Image _leftDigit, _midDigit, _rightDigit;
    private float _time = 0.0f;
    private bool _timerOn = false;

    void Awake() {
        _leftDigit = transform.Find("LeftDigit").GetComponent<Image>();
        _midDigit = transform.Find("MidDigit").GetComponent<Image>();
        _rightDigit = transform.Find("RightDigit").GetComponent<Image>();

        UIActions.OnResetGame += StopWithResetTimer;
        UIActions.OnFirstClick += StartTimer;
        UIActions.OnGameOver += GameOverHandler;
        UIActions.OnOpenSettings += StopWithResetTimer;

        ShowTime();
    }

    void Update() {
        if (_time < 1000 && _timerOn) {
            _time += Time.deltaTime;
            ShowTime();
        }
    }
    
    void GameOverHandler(GridManagerScript.EGameOver status) {
        StopTimer();
    }

    private void ShowTime() {
        _rightDigit.sprite = _digits[(int)_time % 10];
        _midDigit.sprite = _digits[(int)_time / 10 % 10];
        _leftDigit.sprite = _digits[(int)_time / 100 % 10];
    }

    public void StopWithResetTimer() {
        StopTimer();
        ResetTimer();
    }

    public void StopTimer() {
        _timerOn = false;
    }

    public void StartTimer() {
        _timerOn = true;
    }

    public void ResetTimer() {
        _time = 0.0f;
        ShowTime();
    }
}
