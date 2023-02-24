using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static int CoinsCollected;

    [SerializeField] List<AudioClip> _audioClips;

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        CoinsCollected++;
        Debug.Log(CoinsCollected);

        ScoreSystem.Add(100);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        if (_audioClips.Count > 0)
        {
            int _randomNumber = UnityEngine.Random.Range(0, _audioClips.Count - 1);
            AudioClip clip = _audioClips[_randomNumber];
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
        else
            GetComponent<AudioSource>().Play();
    }
}
