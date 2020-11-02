using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Level[] levels; // startTarget shouldn't be here. It will be swapped with the circle which will become a new target
    [Header("Start Player Properties")]
    public Player player;
    public Circle startTarget;
    [Min(0.05f)] public float startSpeed;
    [Range(0f, 360f)] public float startAngle;
    public bool moveClockwise;
    [Header("Start Player Properties")]
    // These fields are made for a bit faster work of update cycle
    private Circle playerCircle;
    private Vector3 playerCirclePosition;

    private float timeSwap;
    private int currentLevel;

    private IEnumerator turnOffPrevLevel;

    private const float timeWaiting = 0.2f;
    private const float passDelta1 = 0.04f;
    private const float passDelta2 = 0.02f;

    private void Start()
    {
        currentLevel = 0;
        turnOffPrevLevel = TurnOffPreviousLevel(currentLevel);

        if (moveClockwise) player.SetStartTarget(startTarget, startSpeed, -1f, startAngle * Mathf.Deg2Rad);
        else player.SetStartTarget(startTarget, startSpeed, 1f, startAngle * Mathf.Deg2Rad);

        playerCircle = startTarget;
        playerCirclePosition = startTarget.transform.position;
        timeSwap = Time.time;
    }

    void Update()
    {
        if (Time.time - timeSwap > timeWaiting)
        {
            ChangeCircle();
        }
    }

    private void ChangeCircle()
    {
        for (int i = 0; i < levels[currentLevel].circles.Length; ++i)
        {
            float distance = Vector3.Magnitude(playerCirclePosition - levels[currentLevel].circles[i].transform.position);
            if (Mathf.Abs(distance - playerCircle.radius - levels[currentLevel].circles[i].radius) < passDelta1)
            {
                Vector3 touchPoint = Vector3.Lerp(playerCirclePosition, levels[currentLevel].circles[i].transform.position, playerCircle.radius / (playerCircle.radius + levels[currentLevel].circles[i].radius));
                if (Vector3.SqrMagnitude(player.transform.position - touchPoint) < passDelta2)
                {
                    playerCircle = levels[currentLevel].circles[i];
                    levels[currentLevel].circles[i] = player.GetCircle();
                    player.ChangeTarget(playerCircle, -1f);
                    playerCirclePosition = playerCircle.transform.position;
                    timeSwap = Time.time;
                    CheckForNextLevel();
                    return;
                }
            }
            else if (Mathf.Abs(distance - Mathf.Abs(playerCircle.radius - levels[currentLevel].circles[i].radius)) < passDelta1)
            {
                Vector3 touchPoint;
                if (playerCircle.radius > levels[currentLevel].circles[i].radius)
                    touchPoint = playerCirclePosition + (levels[currentLevel].circles[i].transform.position - playerCirclePosition) * playerCircle.radius / distance;
                else
                    touchPoint = levels[currentLevel].circles[i].transform.position + (playerCirclePosition - levels[currentLevel].circles[i].transform.position) * levels[currentLevel].circles[i].radius / distance;
                if (Vector3.SqrMagnitude(player.transform.position - touchPoint) < passDelta2)
                {
                    playerCircle = levels[currentLevel].circles[i];
                    levels[currentLevel].circles[i] = player.GetCircle();
                    player.ChangeTarget(playerCircle, 1f);
                    playerCirclePosition = playerCircle.transform.position;
                    timeSwap = Time.time;
                    CheckForNextLevel();
                    return;
                }
            }
        }
    }

    private void CheckForNextLevel()
    {
        if (playerCircle == levels[currentLevel].final)
        {
            levels[currentLevel].Hide();

            StopCoroutine(turnOffPrevLevel);
            turnOffPrevLevel = TurnOffPreviousLevel(currentLevel);
            StartCoroutine(turnOffPrevLevel);

            currentLevel++;
            levels[currentLevel].ShowFadeableObjects();
            int nextLevel = currentLevel + 1;
            if (nextLevel < levels.Length)
            {
                levels[nextLevel].gameObject.SetActive(true);
                levels[currentLevel].ShowCircles();
            }
        }
    }

    private IEnumerator TurnOffPreviousLevel(int level)
    {
        yield return new WaitForSeconds(1f);
        levels[level].gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (startTarget)
            Gizmos.DrawWireSphere(startTarget.transform.position + PolarCoordinateSystem.GetPosition(startAngle * Mathf.Deg2Rad, startTarget.radius), 0.2f);
    }
}
