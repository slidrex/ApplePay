using UnityEngine;

public class AbilityGrid : MonoBehaviour
{
    [SerializeField] private AbilityRepository repository;
    private System.Collections.Generic.List<AbilityGridCell> activeCells = new System.Collections.Generic.List<AbilityGridCell>();
    [SerializeField] private AbilityGridCell cell;
    private void Start()
    {
        repository.ItemAddCallback += OnAbilityAdded;
    }
    private void Destroy() => repository.ItemAddCallback -= OnAbilityAdded;
    private void OnAbilityAdded(int index)
    {
        AbilityGridCell currentCell = Instantiate(cell);
        currentCell.transform.SetParent(transform);
        currentCell.transform.localScale = Vector3.one;
        currentCell.transform.localPosition = Vector3.zero;
        activeCells.Add(currentCell);
    }
    private void Update()
    {
        UpdateCellsMask();
    }
    private void UpdateCellsMask()
    {
        for(int i = 0; i < repository.Items.Length; i++)
        {//performance
        if(repository.Items[i] != null && repository.Items[i].Ability.timeSinceAbilityActivated > 0)
        {
                float fillAmount = repository.Items[i].Ability.timeSinceAbilityActivated / repository.Items[i].Ability.GetCooldown();
                activeCells[i].SetMaskFillAmount(1 - fillAmount);
        }
        }
    }
}
