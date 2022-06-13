using UnityEngine;

public class BallController : MonoBehaviour
{
    public Vector2 velocity = new Vector2(2, 2  );
    public float speed = 2f;
    public Vector2 resetPosition = new Vector2(0, 0);

    private Rigidbody2D rig;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.velocity = velocity * speed;
    }

    public void ResetBallPosition()
    {
        transform.position = resetPosition;
        rig.velocity = new Vector2(2f,2f) * 2f;
    }

    public void SetSpeed(float speed)
    {
        rig.velocity *= speed;
    }
}
