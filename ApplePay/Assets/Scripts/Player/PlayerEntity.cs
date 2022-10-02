using UnityEngine;
using System.Linq;

public class PlayerEntity : Creature, IWavedepent, IEffectUpdateHandler, IDamageDealable
{
    public int AttackDamage {get; set;} = 10;
    [Header("Player Entity")]
    [SerializeField] private GameObject EffectList;
    [HideInInspector] public float ChangeAmount;
    [HideInInspector] public KeyCode ChangeHealthKey;
    private float vignetteIntensity;
    public WaveStatus WaveStatus { get; private set; }
    [SerializeField] private EffectCell effectCell;
    private UnityEngine.Rendering.Universal.Vignette vignette;
    public void AddDamageAttribute()
    {
        GetComponent<Entity>().AddAttribute("attackDamage", new ReferencedAttribute(
            () => AttackDamage,
            val => AttackDamage = (int)val
        ), AttackDamage);
    }
    protected override void Start()
    {
        AddDamageAttribute();
        vignette = FindObjectOfType<UnityEngine.Rendering.Universal.Vignette>();
        base.Start();
    }
    public void SetWaveStatus(WaveStatus waveStatus) => WaveStatus = waveStatus;
    public void OnEffectUpdated()
    {
        for(int i = 0; i < EffectList.transform.childCount; i++)
        {
            Destroy(EffectList.transform.GetChild(i).gameObject);
        }
        for(int i = 0; i < ActiveEffects.Count; i++)
        {
            if(ActiveEffects.ElementAt(i).Value.EffectDisplay.Equals(new PayWorld.EffectController.EffectDisplay()))
                continue;
                
            var obj = Instantiate(effectCell.gameObject, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(EffectList.transform);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<EffectCell>().EffectDisplay = ActiveEffects.ElementAt(i).Value.EffectDisplay;
        }
    }
    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //PayWorld.EffectController.AddEffect(this, "decay", 3, 5f);
            Damage(10, DamageType.Magical, null);
        }

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
}