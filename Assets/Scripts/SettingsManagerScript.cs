using System;
using UnityEngine;
using TMPro;

public class SettingsManagerScript : MonoBehaviour
{
    public static Vector2Int MapSize = new Vector2Int(10, 10);
    public static int TotalMines = 15;
    public static Vector2Int PixelCellSize = new Vector2Int(16, 16);
    public static int CameraScale = 2; // 1 2 4 8
    
    [SerializeField] private TMP_InputField _minesField, _widthField, _heightField;
    private CanvasGroup _canvasGroup; 
    private const int MinWidth = 10;

    private void Awake() {
        _minesField.onValueChanged.AddListener(OnChangedMinesField);
        _widthField.onValueChanged.AddListener(OnChangedWidthField);
        _heightField.onValueChanged.AddListener(OnChangedHeightField);

        _widthField.onEndEdit.AddListener(OnEndEditWidthField);
        _heightField.onEndEdit.AddListener(OnEndEditHeightField);

        _canvasGroup = GetComponent<CanvasGroup>();
        HideMenu();
    }

    public void HideMenu() {
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0;
    }

    public void ActivateMenu() {
        _canvasGroup.interactable = true;
        _canvasGroup.alpha = 1;
    }

    public void SubmitAndHideSettings() {
        MapSize.x = int.TryParse(_widthField.text, out int width) ? width : MapSize.x;
        MapSize.y = int.TryParse(_heightField.text, out int height) ? height : MapSize.y;
        TotalMines = int.TryParse(_minesField.text, out int mines) ? Math.Min(mines, MapSize.x * MapSize.y) : TotalMines;

        HideMenu();
    }

    private void RemoveMinusFromStringStart(ref string input) {
        if (string.IsNullOrEmpty(input) || input[0] != '-') {
            return;
        }
        input = input.Remove(0, 1);
    }

    private void OnChangedMinesField(string text) {
        int width = int.TryParse(_widthField.text, out int w) ? w : MapSize.x;
        int height = int.TryParse(_heightField.text, out int h) ? h : MapSize.y;

        RemoveMinusFromStringStart(ref text);
        _minesField.text = text;

        if (int.TryParse(text, out int mines)) {
            TotalMines = Math.Min(mines, width * height);
            _minesField.text = TotalMines.ToString();
        }
    }

    private void OnChangedWidthField(string text) {
        RemoveMinusFromStringStart(ref text);
        _widthField.text = text;
        
        if (int.TryParse(text, out int width) && width * CameraScale * PixelCellSize.x > Screen.currentResolution.width) {
            int maxWidth = Screen.currentResolution.width / (CameraScale * PixelCellSize.x);
            _widthField.text = maxWidth.ToString();
        }
    }

    private void OnChangedHeightField(string text) {
        int UIPanelPixelHeight = (int)Mathf.Ceil(GameObject.Find("UIPanel").GetComponent<RectTransform>().sizeDelta.y);

        RemoveMinusFromStringStart(ref text);
        _heightField.text = text;

        if (int.TryParse(text, out int height) && CameraScale * (height * PixelCellSize.y + UIPanelPixelHeight) > Screen.currentResolution.height) {
            int maxHeight = (Screen.currentResolution.height - UIPanelPixelHeight * CameraScale) / (PixelCellSize.y * CameraScale);
            _heightField.text = maxHeight.ToString();
        }
    }

    private void OnEndEditWidthField(string text) {
        if (int.TryParse(text, out int width) && width < MinWidth) {
            _widthField.text = MinWidth.ToString();
        }
        OnChangedMinesField(_minesField.text); 
    }

    private void OnEndEditHeightField(string text) {
        if (int.TryParse(text, out int height) && height <= 0) {
            _heightField.text = "1";
        }
        OnChangedMinesField(_minesField.text);
    }
}
