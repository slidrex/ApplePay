using UnityEngine;
public class MenuComponents : MonoBehaviour
{
    [HideInInspector] public bool IsOpen;
    public GameObject[] Elements;
    public void SetActiveElements(bool active, params GameObject[] components)
    {
        SetActive(false, Elements);
        SetActive(active, components);
        foreach(GameObject _gameObject in Elements)
        {
            if(_gameObject.activeSelf == true) return;
        }
        gameObject.SetActive(false);
    }
    private void SetActive(bool isActive, params GameObject[] objects)
    {
        for(int i = 0 ; i < objects.Length; i++)
            objects[i].gameObject.SetActive(isActive);
    }
}
