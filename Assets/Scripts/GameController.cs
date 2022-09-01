using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class GameController : MonoBehaviour
{
    public Level startLevel;
    public List<Level> levels;
    [Header("Start Player Properties")]
    public Player player;
    public SmoothCamera smoothCamera;
    public Circle endCircle;
    [Header("Circle Manipulation")]
    public Slider slider;
    [Min(0f)] public float deltaMultiplier;
    public const float materialScaleMultiplier = 4f;
    private Circle playerCircle;
    private Vector3 playerCirclePosition;

    private float timeSwap;
    private int currentLevel;

    private IEnumerator turnOffPrevLevel;

    private const float timeWaiting = 0.2f;
    private const float passDelta1 = 0.04f;
    private const float passDelta2 = 0.02f;

    private void Awake()
    {
        turnOffPrevLevel = TurnOffPreviousLevel(currentLevel);
    }

    private void Start()
    {
        GameAnalytics.Initialize();
        endCircle.gameObject.SetActive(true);
        SetLevel(levels.IndexOf(startLevel));
        RefreshPlayerTarget(true);
        SetEndCircle(levels[currentLevel].final);
    }

    void Update()
    {
        if (Time.time - timeSwap > timeWaiting)
        {
            ChangeCircle();
        }
        ChangeRadius();
    }

    private void ChangeCircle()
    {
        for (int i = 0; i < levels[currentLevel].circles.Length; ++i)
        {
            if (levels[currentLevel].circles[i] != playerCircle)
            {
                float distance = Vector3.Magnitude(playerCirclePosition - levels[currentLevel].circles[i].transform.position);
                if (Mathf.Abs(distance - playerCircle.radius - levels[currentLevel].circles[i].radius) < passDelta1)
                {
                    Vector3 touchPoint = Vector3.Lerp(playerCirclePosition, levels[currentLevel].circles[i].transform.position, playerCircle.radius / (playerCircle.radius + levels[currentLevel].circles[i].radius));
                    if (Vector3.SqrMagnitude(player.transform.position - touchPoint) < passDelta2)
                    {
                        SetNewTargetCircleWithChecking(levels[currentLevel].circles[i], -1f);
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
                        SetNewTargetCircleWithChecking(levels[currentLevel].circles[i], 1f);
                        return;
                    }
                }
            }
        }
    }

    private void SetNewTargetCircleWithChecking(Circle circle, float newDirection)
    {
        playerCircle = circle;
        CheckForNextLevel();

        RefreshPlayerTarget(false, newDirection);
    }

    private void RefreshPlayerTarget(bool setPositionAndAngle, float newDirection = 1f)
    {
        if (setPositionAndAngle)
        {
            player.SetStartTarget(levels[currentLevel].start, 
                levels[currentLevel].moveClockwise ? -1f : 1f, levels[currentLevel].startAngle * Mathf.Deg2Rad);
        }
        else
        {
            player.ChangeTarget(playerCircle, newDirection);
        }
        playerCirclePosition = playerCircle.transform.position;
        timeSwap = Time.time;
        smoothCamera.SetTargetAndGradientValue(playerCircle.transform, playerCircle.colorGradientValue);
        SetVisualRadius();
    }

    private void SetVisualRadius()
    {
        float clippedValue = playerCircle.GetClippedValue();
        if (clippedValue > -1f)
        {
            slider.gameObject.SetActive(true);
        }
        else
        {
            slider.gameObject.SetActive(false);
        }
    }

    private void CheckForNextLevel()
    {
        if (playerCircle == levels[currentLevel].final)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level " + (currentLevel + 1).ToString());
            levels[currentLevel].Hide();
            endCircle.Hide();

            StopCoroutine(turnOffPrevLevel);
            turnOffPrevLevel = TurnOffPreviousLevel(currentLevel);
            StartCoroutine(turnOffPrevLevel);

            SetLevel(currentLevel + 1);
            RefreshPlayerTarget(false);
        }
    }

    private void SetLevel(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level " + (level + 1).ToString());
        currentLevel = level;
        levels[currentLevel].gameObject.SetActive(true);
        levels[currentLevel].ShowCircles();
        levels[currentLevel].ShowFadeableObjects();
        smoothCamera.SetColorGradient(levels[currentLevel].colorStart, levels[currentLevel].colorEnd,
            levels[currentLevel].foregroundColorStart, levels[currentLevel].foregroundColorEnd);
        playerCircle = levels[currentLevel].start;
        player.ChangeSpeed(levels[currentLevel].levelSpeed);
    }

    private IEnumerator TurnOffPreviousLevel(int level)
    {
        yield return new WaitForSeconds(1f);
        levels[level].gameObject.SetActive(false);
        SetEndCircle(levels[currentLevel].final);
    }

    private void ChangeRadius()
    {
        playerCircle.ChangeRadius(GetDelta() * deltaMultiplier);
        VisualizeRadiusChange();
    }

    private float GetDelta()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            float prevMagnitude = Helper.ClipPixelPosition(touchZero.position - touchZero.deltaPosition - touchOne.position + touchOne.deltaPosition).magnitude;
            float currentMagnitude = Helper.ClipPixelPosition(touchZero.position - touchOne.position).magnitude;
            return currentMagnitude - prevMagnitude;
        }
        else
        {
            return Input.GetAxis("Horizontal") * Time.deltaTime;
        }
    }

    private void VisualizeRadiusChange()
    {
        slider.value = Mathf.Lerp(slider.value, playerCircle.GetClippedValue(), 0.1f);
    }

    private void SetEndCircle(Circle circle)
    {
        if (circle)
        {
            endCircle.transform.position = circle.transform.position;
            endCircle.SetRadius(circle.radius);
            endCircle.Show();
        }
    }
}