using UnityEngine;
using UnityEngine.UI;

public class FlagCounterScript : MonoBehaviour
{
    [SerializeField] private Sprite[] _digits;
    [SerializeField] private Sprite _minus;
    private Image _leftDigit, _midDigit, _rightDigit;

    private void Awake() {
        _leftDigit = transform.Find("LeftDigit").GetComponent<Image>();
        _midDigit = transform.Find("MidDigit").GetComponent<Image>();
        _rightDigit = transform.Find("RightDigit").GetComponent<Image>();

        UIActions.OnChangedFlags += SetValue;

        SetValue(SettingsManagerScript.TotalMines);
    }

    public void SetValue(int flags) {
        if (flags < -99 || flags > 999) {
            return;
        }

        if (flags < 0) {
            flags = -flags;

            _leftDigit.sprite = _minus;
            _rightDigit.sprite = _digits[flags % 10];
            _midDigit.sprite = _digits[flags / 10 % 10];
        }
        else {
            _rightDigit.sprite = _digits[flags % 10];
            _midDigit.sprite = _digits[flags / 10 % 10];
            _leftDigit.sprite = _digits[flags / 100 % 10];
        }
    }
}
