using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleSwitch : MonoBehaviour
{
    [SerializeField] ToggleDirection _startingDirection = ToggleDirection.Center;

    [SerializeField] Sprite _switchLeftSprite;
    [SerializeField] Sprite _switchRightSprite;
    [SerializeField] Sprite _switchCenterSprite;

    [SerializeField] UnityEvent _onSwitchLeft;
    [SerializeField] UnityEvent _onSwitchRight;
    [SerializeField] UnityEvent _onSwitchCenter;

    SpriteRenderer _spriteRenderer;
    ToggleDirection _currentDirection;
    

    enum ToggleDirection
    {
        Left,
        Center,
        Right,
    }

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetToggleDirection(_startingDirection, true);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        var playerRigidBody = player.GetComponent<Rigidbody2D>();
        if (playerRigidBody == null)
            return;

        bool wasOnRight = collision.transform.position.x > transform.position.x;
        bool playerWalkingRight = playerRigidBody.velocity.x > 0;
        bool playerWalkingLeft = playerRigidBody.velocity.x < 0;

        if (wasOnRight && playerWalkingRight)
        {
            SetToggleDirection(ToggleDirection.Right);
        }
        else if (!wasOnRight && playerWalkingLeft)
        {
            SetToggleDirection(ToggleDirection.Left);
        }
    }

    void SetToggleDirection(ToggleDirection direction, bool force = false)
    {
        if (force == false && _currentDirection == direction)
            return;

        _currentDirection = direction;
        switch (direction)
        {
            case ToggleDirection.Left:
                _spriteRenderer.sprite = _switchLeftSprite;
                _onSwitchLeft.Invoke();
                break;
            case ToggleDirection.Center:
                _spriteRenderer.sprite = _switchCenterSprite;
                _onSwitchCenter.Invoke();
                break;
            case ToggleDirection.Right:
                _spriteRenderer.sprite = _switchRightSprite;
                _onSwitchRight.Invoke();
                break;
            default:
                break;
        }
    }

    void OnValidate()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_startingDirection == ToggleDirection.Left)
            _spriteRenderer.sprite = _switchLeftSprite;
        else if (_startingDirection == ToggleDirection.Center)
            _spriteRenderer.sprite = _switchCenterSprite;
        else if (_startingDirection == ToggleDirection.Right)
            _spriteRenderer.sprite = _switchRightSprite;
    }
}
