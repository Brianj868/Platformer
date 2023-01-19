using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBox : MonoBehaviour
{
    [SerializeField] int _totalCoins = 3;
    [SerializeField] Sprite _usedSprite;

    int _remainingCoins;

    private void Start()
    {
        _remainingCoins = _totalCoins;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player == null)
            return;

        if (collision.contacts[0].normal.y > 0 && _remainingCoins > 0)
        {
            Coin.CoinsCollected++;
            _remainingCoins--;
            if (_remainingCoins <= 0)
            {
                GetComponent<SpriteRenderer>().sprite = _usedSprite;
            }
        }
    }
}
