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
            _spriteRenderer.sprite = _switchRightSprite;
        else if (!wasOnRight && playerWalkingLeft)
            _spriteRenderer.sprite = _switchLeftSprite;
    }

    void TurnSwitchRight()
    {
       
        _onSwitchRight.Invoke();
    }

    void TurnSwitchLeft()
    {
        
        _onSwitchLeft.Invoke();
    }
}
