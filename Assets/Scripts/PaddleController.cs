using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    private float defaultScale = 2.5f;

    private int defaultSpeed = 4;
    public int speed = 4;

    public KeyCode upKey;
    public KeyCode downKey;
    private Rigidbody2D rig;

    public void SetPaddleSpeedToDefault() => speed = defaultSpeed;
    public void SetPaddleLengthToDefault() => transform.localScale = new Vector3(transform.localScale.x, defaultScale, transform.localScale.z);
    public IEnumerator MultiplyPaddleSpeedByTwo(int timerInSeconds)
    {
        speed = defaultSpeed * 2;
        yield return new WaitForSeconds(timerInSeconds);
        SetPaddleSpeedToDefault();
    }

    public IEnumerator MultiplyPaddleLengthByTwo(int timerInSeconds)
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
        yield return new WaitForSeconds(timerInSeconds);
        SetPaddleLengthToDefault();
    }

    private void Start()
    {
        defaultSpeed = speed;
        defaultScale = transform.localScale.y;
        Debug.Log(defaultScale);
        rig = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Debug.Log(name + " " + rig.velocity);
        MoveObject(GetInput());
    }

    private Vector2 GetInput()
    {
        if (Input.GetKey(upKey))
        {
            return Vector2.up * speed;
        }
        else if (Input.GetKey(downKey))
        {
            return Vector2.down * speed;
        }

        return Vector2.zero;
    }

    private void MoveObject(Vector2 movement)
    {
        rig.velocity = movement;
    }

    public void PowerUpLength(int timer) { 
        StartCoroutine(MultiplyPaddleLengthByTwo(timer));
    }

    public void PowerUpSpeed(int timer)
    {
        StartCoroutine(MultiplyPaddleSpeedByTwo(timer));
    }
}
