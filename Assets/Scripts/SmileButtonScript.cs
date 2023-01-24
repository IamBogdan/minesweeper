using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SmileButtonScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite _defaultSprite, _winSprite, _loseSprite;
    private Image _img;

    void Awake() {
        UIActions.OnGameOver += GameOverHandler; 
        
        _img = GetComponent<Image>();
        SetDefault();
    }

    void GameOverHandler(GridManagerScript.EGameOver status) {
        if (status == GridManagerScript.EGameOver.Win) {
            SetWin();
        }
        else {
            SetLose();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            UIActions.OnResetGame?.Invoke();
            SetDefault();
        }
    }

    public void SetDefault() {
        _img.sprite = _defaultSprite;
    }

    public void SetWin() {
        _img.sprite = _winSprite;
    }

    public void SetLose() {
        _img.sprite = _loseSprite;
    }
}