using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICoinsCollected : MonoBehaviour
{
    TMP_Text _text;

    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        _text.SetText(Coin.CoinsCollected.ToString());
    }
}
