using UnityEngine;

public class KillOnEnter : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        var Player = collision.GetComponent<Player>();
        if (Player != null)
        {
            Player.ResetToStart();
        }
    }
}