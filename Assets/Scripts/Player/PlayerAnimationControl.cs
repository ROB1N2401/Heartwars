using System;
using System.Collections;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
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
    
    private volatile bool _isTransitionTime = false;

    public void Respawn(Vector3 targetPosition, float startHeight) => StartCoroutine(RespawnCor(targetPosition, startHeight));

    public void FallDown(float negativeHeight) => StartCoroutine(FallDownCor(negativeHeight));

    public void DirectTransition(Vector3 targetPosition) => StartCoroutine(DirectTransitionCor(targetPosition));

    public void ParabolicTransition(Vector3 targetPosition) => StartCoroutine(ParabolicTransitionCor(targetPosition));
    
    private IEnumerator RespawnCor(Vector3 targetPosition, float startHeight)
    {
        yield return new WaitUntil(() => !_isTransitionTime);
        _isTransitionTime = true;

        var velocity = Vector3.zero;
        transform.position = targetPosition + Vector3.up * startHeight;
        
        while ((transform.position - targetPosition).magnitude > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.2f);
            
            yield return null;
        }
        
        transform.position = targetPosition;
        
        _isTransitionTime = false;
    }

    private IEnumerator FallDownCor(float height)
    {
        yield return new WaitUntil(() => !_isTransitionTime);
        _isTransitionTime = true;

        height = -Math.Abs(height);
        float speed = 0f;
        var yTransition = transform.position.y + height;

        while (transform.position.y > yTransition)
        {
            transform.position += Vector3.down * Time.deltaTime * speed;
            speed += 0.98f;
            yield return null;
        }

        _isTransitionTime = false;
    }

    private IEnumerator ParabolicTransitionCor(Vector3 targetPosition)
    {
        StartCoroutine(LookAtGivenObjectCor(targetPosition));
        
        yield return new WaitUntil(() => !_isTransitionTime);
        _isTransitionTime = true;
        
        const float speed = 2f;
        Func<float, float, float> parabola = (x, dist) =>
            -Mathf.Pow(x - dist / 2, 2) / (dist / 2) + dist / 2;
        var startPosition = transform.position;
        var totalDistance = (targetPosition - startPosition).magnitude;
        while ((transform.position - targetPosition).magnitude > .1f)
        {
            var x = Mathf.Lerp(transform.position.x, targetPosition.x, Time.deltaTime * speed);
            var z = Mathf.Lerp(transform.position.z, targetPosition.z, Time.deltaTime * speed);
            var y = parabola(Vector2.Distance(startPosition, new Vector2(z, x)), totalDistance);
            y += transform.position.y;
            transform.position = new Vector3(x, y, z);

            yield return null;
        }

        transform.position = targetPosition;

        _isTransitionTime = false;
    }

    private IEnumerator DirectTransitionCor(Vector3 targetPosition)
    {
        lock (this)
        {
            yield return new WaitUntil(() => !_isTransitionTime);
            yield return LookAtGivenObjectCor(targetPosition);
            
            _isTransitionTime = true;

            var velocity = Vector3.zero;
            while ((transform.position - targetPosition).magnitude > .01f)
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, .05f);

                yield return null;
            }

            transform.position = targetPosition;

            _isTransitionTime = false;
        }
    }
    
    private IEnumerator LookAtGivenObjectCor(Vector3 target)
    {
        yield return new WaitUntil(() => !_isTransitionTime);
        _isTransitionTime = true;

        const float speed = 30f;
        var rotationY = transform.localEulerAngles.y;
        var endRotationY = Quaternion.LookRotation(target - transform.position).eulerAngles.y;

        while (Mathf.Abs(rotationY - endRotationY) > .01f)
        {
            rotationY = Mathf.Lerp(rotationY, endRotationY, Time.deltaTime * speed);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationY, transform.eulerAngles.z);

            yield return null;
        }
        
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, endRotationY, transform.eulerAngles.z);

        _isTransitionTime = false;
    }
}