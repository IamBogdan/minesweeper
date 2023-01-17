using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileButtonScript : MonoBehaviour
{
    
    [SerializeField] private Sprite _defaultSprite, _winSprite, _loseSprite;
    
    public Sprite DefaultSprite { get => _defaultSprite; }
    public Sprite WinSprite { get => _winSprite; }
    public Sprite LoseSprite { get => _loseSprite; }
}