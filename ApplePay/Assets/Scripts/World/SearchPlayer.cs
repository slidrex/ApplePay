using UnityEngine;
using Cinemachine;

public class SearchPlayer : MonoBehaviour
{
    public GameObject FollowObject {get; set;}
    private CinemachineVirtualCamera virtalCamera;
    private void Start()
    {
        FollowObject = GameObject.FindGameObjectWithTag("Player");
        virtalCamera = GetComponent<CinemachineVirtualCamera>();
        virtalCamera.Follow = FollowObject?.transform;
        virtalCamera.m_Lens.NearClipPlane = -1;
    }
    
}
