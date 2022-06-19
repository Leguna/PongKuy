using UnityEngine;
using UnityEngine.Events;

enum PowerUpType { BallSpeed, PaddleLength, PaddleSpeed };

public class PowerUpController : MonoBehaviour
{
    public Collider2D colliderTarget;
    [SerializeField] private float timeToRemove = 10;

    [SerializeField] private PowerUpType powerUpType;

    [SerializeField] private PowerUpManager powerUpManager;
    [SerializeField] private UnityEvent<int> onTriggerCallback;
    [SerializeField] private UnityEvent onRemoveCallback;

    private void Update()
    {
        timeToRemove -= Time.deltaTime;
        if (timeToRemove <= 0)
        {
            onRemoveCallback.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (colliderTarget==collision)
        {
            onTriggerCallback.Invoke((int)powerUpManager.targetPowerUp);
            onRemoveCallback.Invoke();
        }
    }

}
