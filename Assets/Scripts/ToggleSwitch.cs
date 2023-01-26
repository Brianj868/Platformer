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
        {
            SetPosition(true);
        }
        else if (!wasOnRight && playerWalkingLeft)
        {
            SetPosition(false);
        }
    }

    void SetPosition(bool right)
    {
        if (right)
        {
            _spriteRenderer.sprite = _switchRightSprite;
            _onSwitchRight.Invoke();
        }
        else
        {
            _spriteRenderer.sprite = _switchLeftSprite;
            _onSwitchLeft.Invoke();
        }
    }
}
