using UnityEngine;

public class PlayerMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] ToggleMenuItems;
    [SerializeField] private Transform menuComponents;
    [SerializeField, Tooltip("objects that active while the menu is open")] private GameObject[] internalElements;
    public bool IsOpen;
    [SerializeField] private KeyCode menuActivateKey;
    private Creature owner;
    private byte constraintID;
    private Creature.EntityState engagingState;
    private void Start()
    {
        owner = GetComponent<Creature>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(menuActivateKey))
        {
            if(IsOpen == false && owner.IsFree() == false) return;
            
            ToggleMenu();

        }
    }
    private void ToggleMenu()
    {
        IsOpen = !IsOpen;
        menuComponents.gameObject.SetActive(IsOpen);
        DeactivateChilds(menuComponents);
        SetMenuElementsActive(IsOpen, internalElements);
        SetMenuElementsActive(IsOpen, ToggleMenuItems);
        GetComponent<Animator>().SetBool("isMoving", false);

        if(IsOpen) OnTabOpen();
        else OnTabClose();
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
    public void SetMenuElementsActive(bool isActive, GameObject[] objects)
    {
        foreach(GameObject gameObject in objects)
        {
            gameObject.SetActive(isActive);
        }
    }
    ///<summary>Deactivates all transform childs.</summary>
    public void DeactivateChilds(Transform transform)
    {
        foreach(Transform child in transform) 
        {
            child.gameObject.SetActive(false);
        }
    }
}