using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransition : MonoBehaviour
{
    [Header("Rotation options")]
    [SerializeField] [Min(.0001f)] private float rotationSpeed = 30f;
    
    [Header("Direct transition options")]
    [SerializeField] [Min(.0001f)] private float directTransitionTime = .05f;

    [Header("Respawn options")] 
    [SerializeField] [Min(.0001f)] private float respawnSpeed = .2f;
    
    [Header("Flying options")]
    [SerializeField] private float initialSpeed = 0f;
    [SerializeField] private float acceleration = .98f;
    
    public bool IsTransitionTime
    {
        get
        {
            lock (this)
            {
                return _isTransitionTime;
            }
        }
    }
    
    private Queue<IEnumerator> _animationQueue = new Queue<IEnumerator>();
    private volatile bool _isTransitionTime = false;

    private void Start() => StartCoroutine(CoroutineCoordinator());

    private IEnumerator CoroutineCoordinator()
    {
        while (true)
        {
            while (_animationQueue.Count > 0)
                yield return _animationQueue.Dequeue();
            yield return null;
        }
    }
    
    public void Respawn(Vector3 targetPosition, float startHeight) => _animationQueue.Enqueue(RespawnCor(targetPosition, startHeight));

    public void RespawnWithoutAnimationQueue(Vector3 targetPosition, float startHeight) =>
        StartCoroutine(RespawnCor(targetPosition, startHeight));
    
    public void Fly(float yOffset, Vector3 direction, Action preAction = null, Action afterAction = null) =>
        _animationQueue.Enqueue(FlyCor(yOffset, direction, preAction, afterAction));

    public void DirectTransition(Vector3 targetPosition) =>
        _animationQueue.Enqueue(DirectTransitionCor(targetPosition));

    public void ParabolicTransition(Vector3 targetPosition) => 
        _animationQueue.Enqueue(ParabolicTransitionCor(targetPosition));
    
    private IEnumerator RespawnCor(Vector3 targetPosition, float startHeight)
    {
        yield return new WaitUntil(() => !_isTransitionTime);
        _isTransitionTime = true;

        var velocity = Vector3.zero;
        transform.position = targetPosition + Vector3.up * startHeight;
        
        while ((transform.position - targetPosition).magnitude > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, respawnSpeed);
            
            yield return null;
        }
        
        transform.position = targetPosition;
        
        _isTransitionTime = false;
    }

    private IEnumerator FlyCor(float offset, Vector3 direction, Action preAction = null, Action afterAction = null)
    {
        yield return new WaitUntil(() => !_isTransitionTime);
        _isTransitionTime = true;

        offset = Mathf.Abs(offset);
        var speed = initialSpeed;
        var finalPos = transform.position + direction.normalized * offset;
        
        preAction?.Invoke();
        while ((finalPos - transform.position).magnitude > 1f)
        {
            transform.position += direction.normalized * (Time.deltaTime * speed);
            speed += acceleration;
            yield return null;
        }
        afterAction?.Invoke();
        
        _isTransitionTime = false;
    }

    private IEnumerator DirectTransitionCor(Vector3 targetPosition)
    {
        yield return new WaitUntil(() => !_isTransitionTime);
        yield return LookAtGivenObjectCor(targetPosition);

        _isTransitionTime = true;

        var velocity = Vector3.zero;
        while ((transform.position - targetPosition).magnitude > .01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, directTransitionTime);

            yield return null;
        }

        transform.position = targetPosition;

        _isTransitionTime = false;
    }

    private IEnumerator ParabolicTransitionCor(Vector3 targetPosition)
    {
        throw new NotImplementedException();
        
        // yield return new WaitUntil(() => !_isTransitionTime);
        // yield return LookAtGivenObjectCor(targetPosition);
        // _isTransitionTime = true;
        //
        // const float height = 10f;
        // const int count = 10;
        // var startPos = transform.position;
        // var endPos = targetPosition;
        // var midPos = (startPos - endPos) / 2;
        // midPos.y += height;
        //
        // for (var i = 0; i < count; i++)
        // {
        //      
        // }
        //
        // _isTransitionTime = false;
    }

    private IEnumerator LookAtGivenObjectCor(Vector3 target)
    {
        yield return new WaitUntil(() => !_isTransitionTime);
        _isTransitionTime = true;
        
        var rotationY = transform.localEulerAngles.y;
        var endRotationY = Quaternion.LookRotation(target - transform.position).eulerAngles.y;

        while (Mathf.Abs(rotationY - endRotationY) > .01f)
        {
            rotationY = Mathf.Lerp(rotationY, endRotationY, Time.deltaTime * rotationSpeed);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationY, transform.eulerAngles.z);

            yield return null;
        }
        
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, endRotationY, transform.eulerAngles.z);

        _isTransitionTime = false;
    }
}