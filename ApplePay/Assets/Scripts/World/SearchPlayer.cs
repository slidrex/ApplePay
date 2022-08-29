using UnityEngine;
using Cinemachine;

public class SearchPlayer : MonoBehaviour
{
    private GameObject followObject;
    private CinemachineVirtualCamera virtalCamera;
    private void Start()
    {
        followObject = GameObject.FindGameObjectWithTag("Player");
        virtalCamera = GetComponent<CinemachineVirtualCamera>();
        virtalCamera.Follow = followObject?.transform;
        virtalCamera.m_Lens.NearClipPlane = -1;
    }
    
}
