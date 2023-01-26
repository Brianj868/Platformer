using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleSwitch : MonoBehaviour
{
    [SerializeField] Sprite _switchLeftSprite;
    [SerializeField] Sprite _switchRightSprite;
    [SerializeField] UnityEvent _onSwitchLeft;
    [SerializeField] UnityEvent _onSwitchRight;

    SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        bool wasOnRight = collision.transform.position.x > transform.position.x;
        //_spriteRenderer.sprite = wasOnRight ? _switchLeftSprite : _switchRightSprite;

        if (wasOnRight)
        {
            TurnSwitchLeft();
        }
        else
        {
            TurnSwitchRight();
        }
    }

    void TurnSwitchRight()
    {
        _spriteRenderer.sprite = _switchRightSprite;
        _onSwitchRight.Invoke();
    }

    void TurnSwitchLeft()
    {
        _spriteRenderer.sprite = _switchLeftSprite;
        _onSwitchLeft.Invoke();
    }
}
