using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    Vector3 startPosition;
    Vector2 direction = Vector2.up;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime);
        var distance = Vector2.Distance(startPosition, transform.position);
        if (distance >= 2)
        {
            direction *= -1;
        }
    }
}
