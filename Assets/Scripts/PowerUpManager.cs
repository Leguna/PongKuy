using System.Collections.Generic;
using UnityEngine;

public enum TargetPowerUp { None = 0, LeftPaddle = 1, RightPaddle = 2, Ball = 3};

public class PowerUpManager : MonoBehaviour
{
    public List<GameObject> powerUpTemplateList;

    public Transform spawnArea;
    public int maxPowerUpAmount = 2;
    public Vector2 powerUpAreaMin;
    public Vector2 powerUpAreaMax;

    public int spawnInterval = 3;
    private float timer;

    public TargetPowerUp targetPowerUp;
    [SerializeField] private List<GameObject> powerUpList;

    private void Start()
    {
        powerUpList = new List<GameObject>();
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > spawnInterval)
        {
            GenerateRandomPowerUp();
            timer -= spawnInterval;
        }
    }

    public void GenerateRandomPowerUp()
    {
        GenerateRandomPowerUp(new Vector2(Random.Range(powerUpAreaMin.x, powerUpAreaMax.x), Random.Range(powerUpAreaMin.y, powerUpAreaMax.y)));
    }

    public void GenerateRandomPowerUp(Vector2 position)
    {
        if (powerUpList.Count >= maxPowerUpAmount)
        {
            return;
        }

        if (position.x < powerUpAreaMin.x ||
            position.x > powerUpAreaMax.x ||
            position.y < powerUpAreaMin.y ||
            position.y > powerUpAreaMax.y)
        {
            return;
        }

        int randomIndex = Random.Range(0, powerUpTemplateList.Count);

        GameObject powerUp = Instantiate(powerUpTemplateList[randomIndex], new Vector3(position.x, position.y, powerUpTemplateList[randomIndex].transform.position.z), Quaternion.identity, spawnArea);
        powerUp.SetActive(true);

        powerUpList.Add(powerUp);
    }

    public void RemovePowerUp(GameObject powerUp)
    {
        powerUpList.Remove(powerUp);
        Destroy(powerUp);
    }

    public void RemoveAllPowerUp()
    {
        while (powerUpList.Count > 0)
        {
            RemovePowerUp(powerUpList[0]);
        }
    }

    public void SetTargetPowerUp(int target)
    {
        switch ((TargetPowerUp)target)
        {
            case (TargetPowerUp.None):
                targetPowerUp = TargetPowerUp.None;
                break;
            case (TargetPowerUp.LeftPaddle):
                targetPowerUp = TargetPowerUp.LeftPaddle;
                break;
            case (TargetPowerUp.RightPaddle):
                targetPowerUp = TargetPowerUp.RightPaddle;
                break;
            case (TargetPowerUp.Ball):
                targetPowerUp = TargetPowerUp.Ball;
                break;
        }
    }

    public void SetTargetPowerUpByType(TargetPowerUp target)
    {
        targetPowerUp = target;
    }
}
