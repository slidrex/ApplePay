using UnityEngine;

public class StaticAwake : MonoBehaviour
{
    public GameObject[] objects;
    private void Awake()
    {
        foreach(GameObject obj in objects)
            obj.GetComponent<IStaticAwakeHandler>().OnAwake();
    }
    private void Start()
    {
        foreach(GameObject obj in objects)
            obj.GetComponent<IStaticStartHandler>().OnStart();
    }
}
