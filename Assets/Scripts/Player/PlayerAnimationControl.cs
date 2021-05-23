using System;
using System.Collections;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    public bool IsTransitionTime { get; private set; } = false;
    
    public void Respawn(Vector3 targetPosition) => throw new NotImplementedException();

    public void FallDown(float negativeHeight) => StartCoroutine(FallDownCor(negativeHeight));

    public void DirectTransition(Vector3 targetPosition) => StartCoroutine(DirectTransitionCor(targetPosition));

    public void ParabolicTransition(Vector3 targetPosition) => StartCoroutine(ParabolicTransitionCor(targetPosition));

    private IEnumerator FallDownCor(float height)
    {
        yield return new WaitUntil(() => !IsTransitionTime);

        height = -Math.Abs(height);
        IsTransitionTime = true;
        float speed = 0f;
        var yTransition = transform.position.y + height;

        //todo debug
        print(yTransition);

        while (transform.position.y > yTransition)
        {
            transform.position += Vector3.down * Time.deltaTime * speed;
            speed += 0.98f;
            yield return null;
        }

        IsTransitionTime = false;
    }

    private IEnumerator ParabolicTransitionCor(Vector3 targetPosition)
    {
        StartCoroutine(LookAtGivenObjectCor(targetPosition));
        
        yield return new WaitUntil(() => !IsTransitionTime);
        IsTransitionTime = true;
        
        const float speed = 1f;
        
        while ((transform.position - targetPosition).magnitude > .01f)
        {
            transform.position = Parabola(transform.position, targetPosition,1f, Time.deltaTime * speed);

            yield return null;
        }

        transform.position = targetPosition;

        IsTransitionTime = false;
    }

    private IEnumerator DirectTransitionCor(Vector3 targetPosition)
    {
        StartCoroutine(LookAtGivenObjectCor(targetPosition));
        
        yield return new WaitUntil(() => !IsTransitionTime);
        IsTransitionTime = true;
        
        var velocity = Vector3.zero;
        while ((transform.position - targetPosition).magnitude > .01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, .1f);

            yield return null;
        }

        transform.position = targetPosition;

        IsTransitionTime = false;
    }
    
    private IEnumerator LookAtGivenObjectCor(Vector3 target)
    {
        yield return new WaitUntil(() => !IsTransitionTime);
        IsTransitionTime = true;

        const float speed = 14f;
        var rotationY = transform.localEulerAngles.y;
        var endRotationY = Quaternion.LookRotation(target - transform.position).eulerAngles.y;

        while (Mathf.Abs(rotationY - endRotationY) > .01f)
        {
            rotationY = Mathf.Lerp(rotationY, endRotationY, Time.deltaTime * speed);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationY, transform.eulerAngles.z);

            yield return null;
        }
        
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, endRotationY, transform.eulerAngles.z);

        IsTransitionTime = false;
    }

    private Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            var travelDirection = end - start;
            var result = start + t * travelDirection;
            result.y += (-parabolicT * parabolicT + 1) * height;
            return result;
        }
        else
        {
            var travelDirection = end - start;
            var levelDirecteion = end - new Vector3(start.x, end.y, start.z);
            var right = Vector3.Cross(travelDirection, levelDirecteion);
            var up = Vector3.Cross(right, travelDirection);
            if (end.y > start.y) up = -up;
            var result = start + t * travelDirection;
            result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
            return result;
        }
    }
}