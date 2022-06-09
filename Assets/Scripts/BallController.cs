using UnityEngine;

public class BallController : MonoBehaviour
{
    public Vector2 speed = new Vector2(1, 1);
    public Vector2 resetPosition = new Vector2(1, 1);

    private Rigidbody2D rig;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.velocity = speed;
    }

    public void ResetBallPosition()
    {
        transform.position = resetPosition;
    }
}
