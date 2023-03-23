using UnityEngine;

public class Breakable : MonoBehaviour, ITakeDamage
{
    AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Player>() == null)
            return;

        if (collision.contacts[0].normal.y > 0)
            TakeHit();
    }

    private void TakeHit()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();

        if (_audioSource != null)
            _audioSource.Play();

        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void TakeDamage()
    {
        TakeHit();
    }
}
