using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private MenuComponents menuComponents;
    [SerializeField] private GameObject[] toggleActiveElements;
    [SerializeField] private KeyCode activateKey;
    private byte constraintID;
    private void Start() => menuComponents.gameObject.SetActive(false);
    private void Update()
    {
        if(Input.GetKeyDown(activateKey))
            SetComponentActive(!menuComponents.gameObject.activeSelf, toggleActiveElements);
    }
    private void SetComponentActive(bool active, GameObject[] elements)
    {
        menuComponents.gameObject.SetActive(true);
        menuComponents.SetActiveElements(active, elements);
        bool isTabOpen = menuComponents.gameObject.activeInHierarchy;
        GetComponent<Animator>().SetBool("isMoving", false);
        if(isTabOpen == true) OnTabOpen();
        else
            OnTabClose();
    }
    private void OnTabClose() => PayWorld.EffectController.RemoveEffect(GetComponent<Entity>(), ref constraintID);
    private void OnTabOpen() => PayWorld.EffectController.AddEffect(GetComponent<Entity>(), out constraintID, new PayWorld.Effect.EffectProperty(PayWorld.Effect.States.MoveConstraint()));
}