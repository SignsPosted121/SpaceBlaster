using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Ship : MonoBehaviour
{
    public static float World_MinVelocity = 10;
    public static float World_MaxVelocity = 1000;
    public static float World_MaxAngularVelocity = 1f;

    public float MaxVelocity = 200;
    public float Velocity;
    public Vector2 StrafeVelocity;
    public Vector3 AngularVelocity;

    private bool Boost = false;
    private float Forward;
    private Vector2 StrafeDir;
    private Vector3 RotateDirection;

    public float Acceleration;
    public float AngularAcceleration;
    [Range(0, 1)] public float StrafeStrength;

    private Collider collider;

    public void Move(float Forward)
    {
        this.Forward = Forward;
    }

    public void Strafe(Vector2 dir)
	{
        StrafeDir = new Vector2(Mathf.Clamp(dir.x, -1, 1), Mathf.Clamp(dir.y, -1, 1));
	}

    public void Rotate(Vector3 RotateDirection)
    {
        this.RotateDirection = RotateDirection;
    }

    public void ActivateBoost(bool Boost)
	{
        this.Boost = Boost;
	}

	public virtual bool CheckIfColliding()
	{
        var CollisionSize = collider.bounds.size;
        var CenterPoint = collider.bounds.center;
        var FrontPoint = CollisionSize.z / 2 * transform.forward + CenterPoint;
        Debug.DrawLine(FrontPoint, FrontPoint + Velocity * Time.fixedDeltaTime * transform.forward, Color.blue, 0.1f);
        Physics.Raycast(FrontPoint, transform.forward, out RaycastHit hit, Velocity * Time.fixedDeltaTime);
        if (hit.transform != null)
        {
            Destroy(gameObject, 2.3f);
            return true;
        }
        return false;
    }

    protected virtual void Awake()
	{
        collider = GetComponent<Collider>();
	}

	protected virtual void Update()
    {
        /*
        Debug.Log($"Moving at {Velocity} and maxing at {MaxVelocity}");
        Debug.Log($"Rotating at {AngularVelocity.magnitude} and maxing at {MaxAngularVelocity}");
        */

        if (Mathf.Abs(Forward) > 0)
            Velocity += Acceleration * Time.deltaTime * Forward;

        if (RotateDirection.magnitude > 0)
            AngularVelocity += AngularAcceleration * Time.deltaTime * RotateDirection;
        else
            AngularVelocity -= AngularAcceleration * Time.deltaTime * AngularVelocity;

        if (Velocity < World_MinVelocity)
            Velocity = World_MinVelocity;
        else if (Velocity > World_MaxVelocity)
            Velocity = World_MaxVelocity;
        else if (Velocity > MaxVelocity * (Boost ? 1.25f : 1))
            Velocity = Mathf.Lerp(Velocity, MaxVelocity * (Boost ? 1.25f : 1), Time.deltaTime * 10);

        if (AngularAcceleration <= 0 && AngularVelocity.magnitude <= 1)
            AngularVelocity = Vector3.zero;
        if (AngularVelocity.magnitude > World_MaxAngularVelocity)
            AngularVelocity = AngularVelocity.normalized * World_MaxAngularVelocity;

        StrafeVelocity = Vector2.Lerp(StrafeVelocity, StrafeDir, Time.deltaTime * 4);

        var moveDir = transform.forward + transform.TransformDirection(new Vector3(StrafeVelocity.x, StrafeVelocity.y) * StrafeStrength);
        transform.position += Velocity * Time.deltaTime * moveDir;
        transform.Rotate(AngularVelocity, Space.Self);
    }
    
    protected virtual void FixedUpdate()
	{
        CheckIfColliding();
	}
}
