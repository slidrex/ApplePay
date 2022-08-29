using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform followTo;
    private void Update()
    {
        if(followTo != null) Follow();
    }
    private void Follow()
    {
        if(transform.position != followTo.position)
            transform.position = followTo.position;

    }
}
