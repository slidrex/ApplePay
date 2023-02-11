using UnityEngine;

public class AbilityCanvasRenderer : InventoryUIPage, IStaticAwakeHandler, IStaticStartHandler
{
    [SerializeField] private AbilityRepository repository;
    [SerializeField] private Transform slotsContainer;
    [SerializeField] private AbilityIconLayoutCell abilityIcon;
    [SerializeField] private AbilityNodeLayout layout;
    private GameObject selectedAbilityLayout;
    private AbilityIconLayoutCell[] abilityIcons;
    public void OnAwake()
    {
        repository.RepositoryChangeCallback += OnRepositoryUpdated;
    }
    public void OnStart()
    {
        abilityIcons = new AbilityIconLayoutCell[repository.Items.Length];
    }
    protected void OnRepositoryUpdated(int index)
    {
        if(repository.Items[index] == null)
        {
            abilityIcons[index].DestroyCell();
        }
        else
        {
            AbilityIconLayoutCell cell = Instantiate(abilityIcon, slotsContainer);
            AbilityNodeLayout curLayout = Instantiate(layout, transform);
            curLayout.AttachRenderer(this);
            curLayout.BuildLayout(repository.Items[index].Ability);
            curLayout.gameObject.SetActive(false);
            cell.LinkLayout(curLayout);
            cell.LinkRenderer(this);
            abilityIcons[index] = cell;
        }
    }
    public void OnCellClicked(AbilityIconLayoutCell ability)
    {
        if(selectedAbilityLayout != null)
            selectedAbilityLayout.SetActive(false);
        selectedAbilityLayout = ability.GetWorkspace();
        ability.SetActiveWorkspace(true);
    }
}
