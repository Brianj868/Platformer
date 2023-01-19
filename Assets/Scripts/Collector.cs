using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour
{
    [SerializeField] List<Collectible> _collectibles;
    [SerializeField] UnityEvent _onCollectionComplete;

    static Color _gizmoColor = new Color(0.61f, 0.61f, 0.61f, 1);

    TMP_Text _remainingText;

    int _countCollected;
    
    void Start()
    {
        _remainingText = GetComponentInChildren<TMP_Text>();
        foreach (var collectible in _collectibles)
        {
            collectible.OnPickedUp += ItemPickedUp;
        }

        int countRemaining = _collectibles.Count - _countCollected;
        _remainingText?.SetText(countRemaining.ToString());
    }

    // Update is called once per frame
    public void ItemPickedUp()
    {
        _countCollected++;
        int countRemaining = _collectibles.Count - _countCollected;
        _remainingText?.SetText(countRemaining.ToString());

        if (countRemaining > 0)
            return;

        Debug.Log("Got All Gems");
        _onCollectionComplete.Invoke();
    }

    void OnValidate()
    {
        _collectibles = _collectibles.Distinct().ToList();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        foreach (var collectible in _collectibles)
        {
            if (UnityEditor.Selection.activeGameObject == gameObject)
                Gizmos.color = Color.yellow;
            else
                Gizmos.color = _gizmoColor;

            Gizmos.DrawLine(transform.position, collectible.transform.position);
        }
    }
}
