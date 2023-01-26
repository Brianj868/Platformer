using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleSwitch : MonoBehaviour
{
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

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

    void SetToggleDirection(ToggleDirection direction)
    {
        if (_currentDirection == direction)
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

        if (direction == ToggleDirection.Right)
        {
           
        }
        else if (direction == ToggleDirection.Left)
        {
            
        }
    }
}
