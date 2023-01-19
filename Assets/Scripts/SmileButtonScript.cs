using UnityEngine;

public class SmileButtonScript : MonoBehaviour
{
    [SerializeField] private Sprite _defaultSprite, _winSprite, _loseSprite;
    private SpriteRenderer _spriteRenderer;

    public static SmileButtonScript Instance {
        get;
        private set;
    }

    void Awake() {
        if (Instance == null) { 
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetDefault();
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            GridManagerScript.Instance.ResetGame();
            SetDefault();
        }
    }

    public void SetDefault() {
        _spriteRenderer.sprite = _defaultSprite;
    }

    public void SetWin() {
        _spriteRenderer.sprite = _winSprite;
    }

    public void SetLose() {
        _spriteRenderer.sprite = _loseSprite;
    }
}