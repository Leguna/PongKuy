using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalController : MonoBehaviour
{

    public Collider2D ballCollider;
    public UnityEvent onCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == ballCollider)
        {
            onCollision.Invoke();   
        }
    }
}
