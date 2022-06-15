using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum PowerUpType { BallSpeed, PaddleLength, PaddleSpeed };

public class PowerUpController : MonoBehaviour
{

    public Collider2D colliderTarget;
    public float timeToRemove = 10;

    [SerializeField]
    PowerUpType myType;

    public UnityEvent onTriggerCallback;
    public UnityEvent onRemoveCallback;

    private void Update()
    {
        timeToRemove -= Time.deltaTime;
        if(timeToRemove <= 0)
        {
            onRemoveCallback.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == colliderTarget)
        {
            onTriggerCallback.Invoke();
            onRemoveCallback.Invoke();
        }
    }

}
