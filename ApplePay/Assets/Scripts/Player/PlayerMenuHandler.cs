using UnityEngine;

public class PlayerMenuHandler : MonoBehaviour
{
    [SerializeField] private MenuComponents menuComponents;
    [SerializeField] private GameObject[] toggleActiveElements;
    [SerializeField] private KeyCode activateKey;
    private Creature owner;
    private byte constraintID;
    private Creature.EntityState engagingState;
    private void Start()
    {
        menuComponents.gameObject.SetActive(false);
        owner = GetComponent<Creature>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(activateKey))
        {
            if(menuComponents.IsOpen == false && owner.IsFree() == false) return;
            
            SetComponentActive(!menuComponents.gameObject.activeSelf, toggleActiveElements);

        }
    }
    private void SetComponentActive(bool active, GameObject[] elements)
    {
        menuComponents.IsOpen = active;
        menuComponents.gameObject.SetActive(true);
        menuComponents.SetActiveElements(active, elements);
        
        GetComponent<Animator>().SetBool("isMoving", false);
        if(menuComponents.IsOpen == true) OnTabOpen();
        else
            OnTabClose();
    }
    private void OnTabOpen()
    {
        PayWorld.EffectController.AddEffect(owner, out constraintID, new PayWorld.Effect.EffectProperty(PayWorld.Effect.EffectActionPresets.MoveConstraint()));
        engagingState = owner.Engage(null);
    }
    private void OnTabClose()
    {
        PayWorld.EffectController.RemoveEffect(owner, ref constraintID);
        engagingState.Remove();
    }
}