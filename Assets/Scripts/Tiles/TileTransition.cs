using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTransition : MonoBehaviour
{
    private static TileTransition _instance;
    public static TileTransition Instance 
    {
        get
        {
            if (_instance == null)
            {
                var transitionControlGameObject = new GameObject("Transition Control");
                _instance = transitionControlGameObject.AddComponent<TileTransition>();
            }

            return _instance;
        }
    }

    public bool IsTransitioning { get; private set; } = false;

    private Queue<IEnumerator> _animationQueue = new Queue<IEnumerator>();
    private Dictionary<Tile, bool> _tilesInTransition = new Dictionary<Tile, bool>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;

        StartCoroutine(CoroutineCoordinator());
    }

    public void Fly(Tile tile, Vector3 finalPos, float transitionTime, Action preAction = null, Action afterAction = null)
    {
        if(!_tilesInTransition.ContainsKey(tile))
            _tilesInTransition.Add(tile, false);
        
        StartCoroutine(FlyCor(tile, finalPos, transitionTime, preAction, afterAction));
    }

    private IEnumerator FlyCor(Tile tile, Vector3 finalPos, float transitionTime, Action preAction, Action afterAction)
    {
        yield return new WaitUntil(() => !_tilesInTransition[tile]);
        _tilesInTransition[tile] = true;
        IsTransitioning = true;
        
        var velocity = Vector3.zero;
        
        preAction?.Invoke();
        while ((finalPos - tile.transform.position).magnitude > 0.01f)
        {
            tile.transform.position = Vector3.SmoothDamp(tile.transform.position, finalPos, ref velocity, transitionTime);
            yield return null;
        }
        tile.transform.position = finalPos;
        
        afterAction?.Invoke();
        
        _tilesInTransition[tile] = false;
        IsTransitioning = false;
    }
    
    private IEnumerator CoroutineCoordinator()
    {
        while (true)
        {
            while (_animationQueue.Count > 0)
            {
                yield return _animationQueue.Dequeue();
            }
            yield return null;
        }
    }
}
