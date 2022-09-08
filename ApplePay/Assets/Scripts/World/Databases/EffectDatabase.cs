using System.Collections.Generic;
using PayWorld.Effect;
public class EffectDatabase
{
    public byte Level;
    public const string EffectIconPath = "Assets/Sprites/EffectIcons/";
    private TextConfigurationDatabase textConfigDatabase = (TextConfigurationDatabase)PayDatabase.GetDatabase("text_config"); 
    public EffectDatabase(byte level)
    {
        Level = level;
    }
    ///<summary>Gets sprite by name (name.extension). </summary>
    private UnityEngine.Sprite GetSprite(string name) => (UnityEngine.Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(EffectIconPath + name, typeof(UnityEngine.Sprite));

    private Dictionary<string, EffectTemplate> effects;
    public Dictionary<string, EffectTemplate> Effects 
    {
        get
        {
            effects = new Dictionary<string, EffectTemplate>
            {
                ["slowness"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(GetSprite("Slowness.png"), "Slowness", "Movement speed decreased by " + (Level * 0.1f) * 100 + "%.", Pay.Functions.Generic.RomanConverter(Level), new Pay.UI.UIManager.TextField(textConfigDatabase.Find("lore"), "Agent pidorasik makes you slow)))")), 10, States.VelocityChanger(-0.1f * Level)),
                ["speed"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(GetSprite("Slowness.png"), "Speed", "Movement speed increases by " + (Level * 0.1f) * 100 + "%.", Pay.Functions.Generic.RomanConverter(Level)), byte.MaxValue, States.VelocityChanger(0.1f * Level)),
                ["move_constraint"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(GetSprite("Interact.png"), "Move Constraint", "Constraints your movement abilities.", Pay.Functions.Generic.RomanConverter(Level)), 1, States.MoveConstraint()),
                ["speed_hack"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(GetSprite("Interact.png"), "Hack Haste", "Increases hack speed by "+ (Level * 0.1f) * 100 + "%.", Pay.Functions.Generic.RomanConverter(Level)), byte.MaxValue, States.HoldingHackSpeedChanger(0.1f * Level)),
                ["strength"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Strength", "Increases attack damage.", ""), byte.MaxValue, States.Strength(Level * 0.1f)),
                ["reverse_control"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Reverse", "Reverses your control.", ""), 1, States.VelocityReverser()),
                ["instant_heal"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Instant heal", "", null), byte.MaxValue, States.ChangeHealth(Level * 5)),
                ["instant_damage"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Instant damage", "", null), byte.MaxValue, States.ChangeHealth(Level * -5)),
                ["regeneration"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Regeneration", "Restores " + 5 + " health every " + 5/Level +" seconds.",  Pay.Functions.Generic.RomanConverter(Level)), byte.MaxValue, States.ChangeHealth(5, 5/Level)),
                ["regeneration%"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Regeneration", "Restores " + 5 + "% health every " + 5/Level +" seconds.",  Pay.Functions.Generic.RomanConverter(Level)), byte.MaxValue, States.ChangeHealth(5f, 5/Level)),
                ["decay"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Decay", "Loses " + 2 + " health every " + 5/Level +" seconds.",  Pay.Functions.Generic.RomanConverter(Level)), byte.MaxValue, States.ChangeHealth(-2, 5/Level)),
                ["decay%"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Decay", "Loses " + 5 + "% health every " + 5/Level +" seconds.",  Pay.Functions.Generic.RomanConverter(Level)), byte.MaxValue, States.ChangeHealth(-0.05f, 5/Level))
            };
            return effects;
        }
    }
    ///<summary>
    ///Finds effect sprite by default path ("Assets/Sprites/EffectIcons/").
    ///</summary>
    public static UnityEngine.Sprite FindEffectSprite(string file)
    {
        return (UnityEngine.Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(EffectIconPath + file, typeof(UnityEngine.Sprite));
    }
    
}
public struct EffectTemplate
{
    public EffectTemplate(PayWorld.EffectController.EffectDisplay display, byte maxLevel, params StateEffect[] states)
    {
        StateEffects = states;
        TemplateDisplay = display;
        MaxLevel = maxLevel;
    }
    public byte MaxLevel;
    public PayWorld.EffectController.EffectDisplay TemplateDisplay;
    public StateEffect[] StateEffects;
}