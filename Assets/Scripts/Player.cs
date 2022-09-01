using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed;
    private float direction = 1f;
    private float angularPositionRadians;
    private bool movingAllowed;

    private Circle target;
    private float deviationFromTargetRadius; // When switching the target, we can get a deviation from new target's radius. 
    // It will be used in a correct evaluation of position
    private float dampVelocity; // We will damp deviation value, for that we need a velocity variable

    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed * PolarCoordinateSystem.DualPI;
    }

    public void SetStartTarget(Circle newTarget, float startDirection, float newAngularPosition)
    {
        target = newTarget;
        deviationFromTargetRadius = 0f;
        angularPositionRadians = newAngularPosition;
        transform.position = target.transform.position + 
                             PolarCoordinateSystem.GetPosition(angularPositionRadians, target.radius);
        direction = startDirection;
        movingAllowed = true;
    }

    public void ChangeTarget(Circle newTarget, float newDirection)
    {
        target = newTarget;
        Vector3 vector3 = transform.position - target.transform.position;
        deviationFromTargetRadius = vector3.magnitude - target.radius;
        angularPositionRadians = (float)Math.Atan2(vector3.y, vector3.x);
        direction *= newDirection;
    }

    public Circle GetCircle()
    {
        return target;
    }

    private void ChangePosition()
    {
        if (movingAllowed)
        {
            angularPositionRadians = (angularPositionRadians + speed * direction * Time.deltaTime / target.radius) % PolarCoordinateSystem.DualPI;
            transform.position = target.transform.position + 
                                 PolarCoordinateSystem.GetPosition(angularPositionRadians, target.radius + deviationFromTargetRadius);
            transform.rotation = Quaternion.Euler(0f, 0f, angularPositionRadians * Mathf.Rad2Deg - 90f + direction * 90f);
        }
        deviationFromTargetRadius = Mathf.SmoothDamp(deviationFromTargetRadius, 0f, ref dampVelocity, 0.2f);
    }

    private void Update()
    {
        ChangePosition();
    }
}
