using UnityEngine;

public class SmileButtonScript : MonoBehaviour
{
    private enum EType {
        Default,
        Win,
        Lose
    }

    [SerializeField] private Sprite _defaultSprite, _winSprite, _loseSprite;
    private SpriteRenderer _spriteRenderer;
    private EType _type;

    void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetDefault();
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            switch (_type) {
                case EType.Default:
                    Debug.Log("SmileButton (click): EType.Default");
                    break;
                case EType.Win:
                    Debug.Log("SmileButton (click): EType.Win");
                    break;
                case EType.Lose:
                    Debug.Log("SmileButton (click): EType.Lose");
                    break;
            }
        }
    }

    public void SetDefault() {
        _spriteRenderer.sprite = _defaultSprite;
        _type = EType.Default;
    }

    public void SetWin() {
        _spriteRenderer.sprite = _winSprite;
        _type = EType.Win;
    }

    public void SetLose() {
        _spriteRenderer.sprite = _loseSprite;
        _type = EType.Lose;
    }
}