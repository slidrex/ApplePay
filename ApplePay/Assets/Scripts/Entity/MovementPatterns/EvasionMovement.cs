using System.Collections.Generic;
using UnityEngine;

public class EvasionMovement : MovementPattern
{
    public float SaveDistance;
    public float MinObstacleDist;
    public override void OnUpdate()
    {
        RaycastHit2D[] ray = Physics2D.RaycastAll((Vector2)CurrentTransform.position, MovementVector, MinObstacleDist);
        foreach(RaycastHit2D rayhit in ray)
        {
            if((rayhit.collider.transform != CurrentTransform && !rayhit.collider.isTrigger) || MovementVector == Vector2.zero){
                MovementVector = GetVector(CurrentTransform.position, Target.position);
                break;
            } 
        } 
    }
    private Vector2 GetVector(Vector2 current, Vector2 avoid)
    {
        List<Vector2> dirList = new List<Vector2>();
        Vector2 currentVector = Vector2.zero;

        for(int i = 0; i < 360; i += 4) 
        {
            Vector2 tempVec = new Vector2(Mathf.Cos(i), Mathf.Sin(i));
            RaycastHit2D[] rays = Physics2D.RaycastAll(current, tempVec, MinObstacleDist);
            
            foreach(RaycastHit2D ray in rays)
            {
                if(rays.Length <= 1) 
                {
                    dirList.Add(tempVec);
                    break;
                }
            }
        }
        if(dirList.Count == 0) return MovementVector;
        return dirList[Random.Range(0, dirList.Count)];
    }
    public override void OnSpeedUpdate() => UpdateRigidbodyVector();
}