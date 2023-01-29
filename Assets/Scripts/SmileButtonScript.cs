using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SmileButtonScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite _defaultSprite, _winSprite, _loseSprite, _settingsSprite;
    [SerializeField] private SettingsManagerScript _settingsManager;
    private Image _img;
    private bool _isSettings = false;

    private void Awake() {
        UIActions.OnGameOver += GameOverHandler;
        
        _img = GetComponent<Image>();
        SetDefault();
    }

    private void GameOverHandler(GridManagerScript.EGameOver status) {
        if (status == GridManagerScript.EGameOver.Win) {
            SetWin();
        }
        else {
            SetLose();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (_isSettings) {
            _settingsManager.SubmitAndHideSettings();
            UIActions.OnExitSettings?.Invoke();
            SetDefault();
            _isSettings = false;
        }
        else if (eventData.button == PointerEventData.InputButton.Left) {
            UIActions.OnResetGame?.Invoke();
            SetDefault();
        }
        else if (eventData.button == PointerEventData.InputButton.Right) { // open settings
            UIActions.OnOpenSettings?.Invoke();
            SetSettings();
            _settingsManager.ActivateMenu();
            _isSettings = true;
        }
    }

    private void SetDefault() {
        _img.sprite = _defaultSprite;
    }

    private void SetWin() {
        _img.sprite = _winSprite;
    }

    private void SetLose() {
        _img.sprite = _loseSprite;
    }

    private void SetSettings() {
        _img.sprite = _settingsSprite;
    }
}