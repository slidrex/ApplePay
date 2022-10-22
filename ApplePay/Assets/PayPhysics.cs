using UnityEngine;

public class PayPhysics : MonoBehaviour
{
    public float Mass = 1f;
    public float Friction = 0.2f;
    [ReadOnly, SerializeField] private Vector2 targetForce;
    public PayPhysicsBounds Collider;
    public PayPhysicsBounds Hitbox;
    public Vector2 CheckPoint;
    public Vector2 AddFo;
    public void AddForce(Vector2 force)
    {
        targetForce += force/Mass;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) AddForce(AddFo);
        Vector2 checkPPos = (Vector2)transform.position + CheckPoint;
        print(Hitbox.IsInside(transform.position, checkPPos));
    }
    private void FixedUpdate()
    {
        transform.position += (Vector3)targetForce * Time.fixedDeltaTime;
        targetForce = Vector2.MoveTowards(targetForce, Vector2.zero, Friction * Time.fixedDeltaTime);
        Collider2D[] poses = FindObjectsOfType<Collider2D>();
        for(int i = 0; i < poses.Length; i++)
        {
            print(poses[i]);
            if(Collider.IsInside(transform.position, poses[i].ClosestPoint(transform.position)))
            {
                
                targetForce = Vector2.Reflect(targetForce, (poses[i].ClosestPoint(transform.position) - (Vector2)transform.position).normalized);
                print("Reflected!");
            }
        }
    }
    public void OnHitboxCollision()
    {
        
    }
    public void OnCollision()
    {

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + CheckPoint, 0.05f);
        DrawColliderGizmos();
        DrawHitboxGizmos();
    }
    private void DrawColliderGizmos()
    {
        Gizmos.color = Color.cyan;
        if(Collider.PolygonPositions.Length >= 2)
        {
            for(int i = 0; i < Collider.PolygonPositions.Length - 1; i++) Gizmos.DrawLine((Vector2)transform.position + Collider.Offset + Collider.PolygonPositions[i], (Vector2)transform.position + Collider.Offset + Collider.PolygonPositions[i + 1]);
            if(Collider.PolygonPositions.Length >= 3) Gizmos.DrawLine((Vector2)transform.position + Collider.Offset + Collider.PolygonPositions[0], (Vector2)transform.position + Collider.Offset + Collider.PolygonPositions[Collider.PolygonPositions.Length - 1]);

        }
    }
    private void DrawHitboxGizmos()
    {
        Gizmos.color = Color.white;
        if(Hitbox.PolygonPositions.Length >= 2)
        {
            for(int i = 0; i < Hitbox.PolygonPositions.Length - 1; i++) Gizmos.DrawLine((Vector2)transform.position + Hitbox.Offset + Hitbox.PolygonPositions[i], (Vector2)transform.position + Hitbox.Offset + Hitbox.PolygonPositions[i + 1]);
            if(Hitbox.PolygonPositions.Length >= 3) Gizmos.DrawLine((Vector2)transform.position + Hitbox.Offset + Hitbox.PolygonPositions[0], (Vector2)transform.position + Hitbox.Offset + Hitbox.PolygonPositions[Hitbox.PolygonPositions.Length - 1]);
        }

    }
}
[System.Serializable]

public struct PayPhysicsBounds
{
    public Vector2[] PolygonPositions;
    public bool IsRigid;
    public Vector2 Offset;
}
public static class PayPhysicsExtension
{
    public static bool IsInside(this PayPhysicsBounds bounds, Vector2 zeroPosition, Vector2 point)
    {
        int intersectionCount = 0;
        zeroPosition += bounds.Offset;
        for(int i = 0; i < bounds.PolygonPositions.Length - 1; i++)
        {
            if(CheckRightIntersection(zeroPosition + bounds.PolygonPositions[i], zeroPosition + bounds.PolygonPositions[i + 1], point))
            {
                intersectionCount++;
            }
        }
        if(bounds.PolygonPositions.Length >= 3)
        {
            if(CheckRightIntersection(zeroPosition + bounds.PolygonPositions[0], zeroPosition + bounds.PolygonPositions[bounds.PolygonPositions.Length - 1], point))
            {
                intersectionCount++;
            }
        }
        return intersectionCount % 2 == 0 ? false : true;
        
    }
    private static bool CheckRightIntersection(Vector2 first, Vector2 second, Vector2 checkPoint) 
    {
        float d1, d2;
        float a1, b1, b2, c1, c2;


        a1 = second.y - first.y;
        b1 = first.x - second.x;
        c1 = (second.x * first.y) - (first.x * second.y);


        d1 = (a1 * checkPoint.x) + (b1 * checkPoint.y) + c1;
        d2 = (a1 * 1000) + (b1 * checkPoint.y) + c1;


        if (d1 * d2 > 0) return false;


        b2 = checkPoint.x - 1000;
        c2 = (1000 * checkPoint.y) - (checkPoint.x * checkPoint.y);

        d1 = (b2 * first.y) + c2;
        d2 = (b2 * second.y) + c2;


        if (d1 * d2 > 0) return false;

        if ((a1 * b2) == 0.0f) return false;

        return true;
    }
}