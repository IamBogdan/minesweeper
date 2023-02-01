using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private Sprite[] _digits;
    private Image _leftDigit, _midDigit, _rightDigit;
    private float _time = 0f;
    private Timer _timer;

    private void Awake() {
        _leftDigit = transform.Find("LeftDigit").GetComponent<Image>();
        _midDigit = transform.Find("MidDigit").GetComponent<Image>();
        _rightDigit = transform.Find("RightDigit").GetComponent<Image>();

        UIActions.OnResetGame += StopWithResetTimer;
        UIActions.OnFirstClick += StartTimer;
        UIActions.OnGameOver += GameOverHandler;
        UIActions.OnOpenSettings += StopWithResetTimer;

        ShowTime();

        _timer = new Timer(1000);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
    }

    private void OnTimedEvent(System.Object source, ElapsedEventArgs e) {
        if (_time < 999) { 
            _time++;
        }
    }

    private void Update() {
        ShowTime();
    }
    
    private void GameOverHandler(GridManagerScript.EGameOver status) {
        StopTimer();
    }

    private void ShowTime() {
        int time = (int)_time;

        _rightDigit.sprite = _digits[time % 10];
        _midDigit.sprite = _digits[time / 10 % 10];
        _leftDigit.sprite = _digits[time / 100 % 10];
    }

    private void StopWithResetTimer() {
        StopTimer();
        ResetTimer();
    }

    private void StopTimer() {
        _timer.Enabled = false;
    }

    private void StartTimer() {
        _timer.Enabled = true;
    }

    private void ResetTimer() {
        _time = 0f;
        ShowTime();
    }
}
