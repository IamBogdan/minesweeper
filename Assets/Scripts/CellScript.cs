using UnityEngine;

public class CellScript : MonoBehaviour
{
    public enum EType {
        Safe,
        Mine
    }

    public enum EValue {
        Empty,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Mine,
        ExplodedMine,
        Flag,
        Unknown
    }
    
    public static Vector2Int PixelCellSize = new Vector2Int(16, 16);

    public Vector2Int Position { get => _position; }
    public EType Type { get => _type; }
    public bool IsRevealed { get => _isRevealed; }
    public bool IsFlagged { get => _isFlagged; }
    
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Vector2Int _position;
    [SerializeField] private EType _type;
    private SpriteRenderer _spriteRenderer;
    private bool _isFlagged;
    private bool _isRevealed;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver() {
        if (_isRevealed) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (_isFlagged) {
                return;
            }
            GridManagerScript.Instance.LeftClickHandler(this);
        }

        if (Input.GetMouseButtonDown(1)) {
            GridManagerScript.Instance.RightClickHandler(this);
        }
    }

    private void SetSprite(EValue value) {
        _spriteRenderer.sprite = sprites[(int)value];
    }
    
    public void SwapFlag() {
        if (_isRevealed) {
            return;
        }

        if (_isFlagged) {
            SetSprite(EValue.Unknown);
        }
        else {
            SetSprite(EValue.Flag);
        }

        _isFlagged = !_isFlagged;
    }

    public void Init(Vector2Int position, EType type) {
        _position = position;
        _type = type;

        _isFlagged = false;
        _isRevealed = false;

        SetDefaultFlag();
        SetSprite(EValue.Unknown);
    }

    public Sprite GetSprite() {
        return _spriteRenderer.sprite;
    }

    public EType Reveal(EValue mineCount) {
        if (_isRevealed) {
            return _type;
        }
        _isRevealed = true;

        if (_type == EType.Mine) {
            SetSprite(EValue.ExplodedMine);
        }
        else {
            SetSprite(mineCount);
        }
        return _type;
    }

    public void RevealIfMine() {
        if (_isRevealed || _type == EType.Safe) {
            return;
        }

        _isRevealed = true;
        SetSprite(EValue.Mine);
    }

    public void SetSafe() {
        _type = EType.Safe;
    }

    public void SetMine() {
        _type = EType.Mine;
    }

    public void SetDefaultFlag() {
        _spriteRenderer.color = Color.white;
    }

    public void SetWrongFlag() {
        _spriteRenderer.color = new Color(1.0f, 200 / 255.0f, 1.0f);
    }

    public void Destroy() {
        Destroy(gameObject);
    }
}