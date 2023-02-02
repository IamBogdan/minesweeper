using System;
using UnityEngine;
using UnityEngine.UI;

public class FlagCounterScript : MonoBehaviour
{
    [SerializeField] private Sprite[] _digitSprites;
    [SerializeField] private Sprite _minusSprite;
    private Image _leftDigit, _midDigit, _rightDigit;

    private void Awake() {
        _leftDigit = transform.Find("LeftDigit").GetComponent<Image>();
        _midDigit = transform.Find("MidDigit").GetComponent<Image>();
        _rightDigit = transform.Find("RightDigit").GetComponent<Image>();

        UIActions.OnChangedFlags += SetValue;

        SetValue(SettingsManagerScript.TotalMines);
    }

    private void SetValue(int flags) {
        flags = Math.Min(flags, 999);
        flags = Math.Max(flags, -99);
        
        bool isNegative = flags < 0;
        flags = isNegative ? -flags : flags;

        _leftDigit.sprite = isNegative ? _minusSprite : _digitSprites[flags / 100 % 10];
        _midDigit.sprite = _digitSprites[flags / 10 % 10];
        _rightDigit.sprite = _digitSprites[flags % 10];
    }
}
