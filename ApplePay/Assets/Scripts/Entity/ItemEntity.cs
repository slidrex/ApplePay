using UnityEngine;
public abstract class ItemEntity : Entity
{
    [HideInInspector] public int StoredHealth;
    [SerializeField] protected Color32 maxHealthColor = Color.white;
    [SerializeField] protected Color32 minHealthColor = Color.black;
    protected override void Awake()
    {
        base.Awake();
        StoredHealth = MaxHealth;
    }
    protected override void Start()
    {
        base.Start();
        CurrentHealth = StoredHealth;
        ColorUpdate();
    }
    public override void ChangeHealth(int changeAmount)
    {
        ColorUpdate();
        base.ChangeHealth(changeAmount);
    }
    private void ColorUpdate() => SpriteRenderer.color = Color32.Lerp(minHealthColor,maxHealthColor, (float)CurrentHealth/MaxHealth);
}
