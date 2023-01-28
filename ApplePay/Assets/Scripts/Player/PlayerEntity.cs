using UnityEngine;
using System.Linq;
using Pay.UI;

public class PlayerEntity : Creature, IWavedepent, IEffectUpdateHandler, IDamageDealable, IUIHolder
{
    public new PlayerMovement Movement => (PlayerMovement)Movement;
    public int AttackDamage { get; set; } = 10;
    [Header("Player Entity")]
    [SerializeField] private Transform EffectList;
    [HideInInspector] public float ChangeAmount;
    [HideInInspector] public KeyCode ChangeHealthKey;
    private float vignetteIntensity;
    public WaveStatus WaveStatus { get; set; }

    [SerializeField] private EffectCell effectCell;
    private UnityEngine.Rendering.Universal.Vignette vignette;
    [SerializeField] private UIHolder holder;
    public VirtualBase test = new VirtualBase(0f);
    [SerializeField] private NavigatorMap navigator;
    [SerializeField] private Sprite currentRoomImage;
    private GameObject activeNavigatorRoomIcon;
    [SerializeField] private RoomRenderAnchor renderAnchor;
    protected override void Awake()
    {
        WaveController.SetupWaveEntity(this, this, 2.0f);
        base.Awake();
    }
    public void AddDamageAttribute()
    {
        this.AddAttribute("attackDamage", new FloatRef(
            () => AttackDamage,
            val => AttackDamage = (int)val
        ), AttackDamage);
    }
    protected override void Start()
    {
        AddDamageAttribute();
        vignette = FindObjectOfType<UnityEngine.Rendering.Universal.Vignette>();
        navigator.Clear();
        
        base.Start();
        renderAnchor.UnrenderStack();
        renderAnchor.RenderRoom(CurrentRoom);
    }
    public override void OnRoomChanged(Room room, Room oldRoom)
    {
        RenderNavigator(room);
        renderAnchor.UnrenderRoom(oldRoom);
        renderAnchor.RenderRoom(room);
    }
    private void RenderNavigator(Room initialRoom)
    {
        if(activeNavigatorRoomIcon != null) Destroy(activeNavigatorRoomIcon);
        Vector2 gridPosition = initialRoom.GridPosition;
        NavigatorMap.NavigatorElement element = navigator.RenderElement(gridPosition);
        activeNavigatorRoomIcon = element.PushImage(currentRoomImage, Vector2.one * 1.2f);
        TryRenderPosition(gridPosition + Vector2.up);
        TryRenderPosition(gridPosition + Vector2.down);
        TryRenderPosition(gridPosition + Vector2.right);
        TryRenderPosition(gridPosition + Vector2.left);
        TryRenderPath(gridPosition, gridPosition + Vector2.up);
        TryRenderPath(gridPosition, gridPosition + Vector2.down);
        TryRenderPath(gridPosition, gridPosition + Vector2.right);
        TryRenderPath(gridPosition, gridPosition + Vector2.left);
    }
    private void TryRenderPosition(Vector2 position)
    {
        if(navigator.RoomSpawner.FilledCells.ContainsKey(position))
        {
            navigator.RenderElement(position);
        }
    }
    private void TryRenderPath(Vector2 first, Vector2 second)
    {
        if(navigator.RoomSpawner.FilledCells.TryGetValue(first, out RoomSpawner.SpawnerRoom froom) && navigator.RoomSpawner.FilledCells.TryGetValue(second, out RoomSpawner.SpawnerRoom sroom))
        {
            if(RoomExtension.AreRoomsConnected(froom.room, sroom.room))
            {
                navigator.RenderPath(first, second);
            }
        }
    }
    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(ChangeHealthKey)) ChangeHealth((int)ChangeAmount);
    }
    public override void ChangeHealth(int changeAmount)
    {
        base.ChangeHealth(changeAmount);
        vignetteIntensity = (float)CurrentHealth / (float)MaxHealth;
        vignette.intensity = new UnityEngine.Rendering.ClampedFloatParameter(Mathf.Lerp(0.5f, 1f, 1 - vignetteIntensity), CurrentHealth, MaxHealth);
    }
    protected override void Die(Creature killer)
    {
        base.Die(killer);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    protected override void OnInvulnerability()
    {
        Color32 tempColor = SpriteRenderer.color;
        tempColor.a = (byte)(Mathf.Abs(Mathf.Sin(TimeSinceInvulnerability * 7)) * 160);
        SpriteRenderer.color = tempColor;
    }
    protected override void OnInvulnerabilityEnd() => SpriteRenderer.color = new Color32(255, 255, 255, 255);
    public void OnEffectUpdated()
    {
        for(int i = 0; i < EffectList.childCount; i++)
        {
            Destroy(EffectList.GetChild(i).gameObject);
        }
        for(int i = 0; i < ActiveEffects.Count; i++)
        {
            if(ActiveEffects.ElementAt(i).Value.EffectDisplay.Equals(new PayWorld.EffectController.EffectDisplay()))
                continue;
                
            var obj = Instantiate(effectCell, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(EffectList);
            obj.transform.localScale = Vector3.one;
            obj.EffectDisplay = ActiveEffects.ElementAt(i).Value.EffectDisplay;
        }
    }
    public UIHolder GetHolder()
    {
        return holder;
    }
}