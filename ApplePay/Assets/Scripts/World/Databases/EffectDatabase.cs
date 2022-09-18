using System.Collections.Generic;
using PayWorld.Effect;
public class EffectDatabase
{
    public byte Level;
    internal struct EffectColor
    {
        internal const string Negative = "#FF7F7F";
        internal const string Positive = "#9EFFB6";
        internal const string Neutral = "#EABF46";

    }
    public const string EffectIconPath = "Assets/Sprites/EffectIcons/";
    private TextConfigurationDatabase textConfigDatabase = (TextConfigurationDatabase)PayDatabase.GetDatabase("text_config"); 
    public EffectDatabase(byte level)
    {
        Level = level;
    }
    ///<summary>Gets sprite by name (name.extension). </summary>
    private UnityEngine.Sprite GetSprite(string name) => (UnityEngine.Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(EffectIconPath + name, typeof(UnityEngine.Sprite));
    private string ColorizeText(string text, string colorHex) => Pay.Functions.String.SetRichTextTag(text, "color", colorHex);
    private string ColorizeRoman(byte level, string colorHex) => ColorizeText(Pay.Functions.Generic.RomanConverter(level), colorHex);
    private Dictionary<string, EffectTemplate> effects;
    public Dictionary<string, EffectTemplate> Effects 
    {
        get
        {
            effects = new Dictionary<string, EffectTemplate>
            {
                ["slowness"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(GetSprite("Slowness.png"), "Slowness", "Movement speed decreased by " + (Level * 0.1f) * 100 + "%.", ColorizeRoman(Level, EffectColor.Negative), new Pay.UI.UIManager.TextField(textConfigDatabase.Find("lore"), "Agent pidorasik makes you slow)))")), 10, new EffectProperty(States.VelocityChanger(-0.1f * Level), "movementSpeed")),
                ["speed"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(GetSprite("Slowness.png"), "Speed", "Movement speed increases by " + (Level * 0.1f) * 100 + "%.", ColorizeRoman(Level, EffectColor.Positive)), byte.MaxValue, new EffectProperty(States.VelocityChanger(0.1f * Level), "movementSpeed")),
                ["move_constraint"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(GetSprite("Interact.png"), "Move Constraint", "Constraints your movement abilities.", ColorizeRoman(Level, EffectColor.Negative)), 1, new EffectProperty(States.MoveConstraint(), "movementStatus")),
                ["speed_hack"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(GetSprite("Interact.png"), "Hack Haste", "Increases hack speed by "+ (Level * 0.1f) * 100 + "%.", ColorizeRoman(Level, EffectColor.Positive)), byte.MaxValue, new EffectProperty(States.HoldingHackSpeedChanger(0.1f * Level), "hackingSpeed")),
                ["strength"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Strength", "Increases attack damage.", ""), byte.MaxValue, new EffectProperty(States.Strength(Level * 0.1f), "attackDamage")),
                //["reverse_control"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Reverse", "Reverses your control.", ""), 1, new EffectProperty(States.VelocityReverser(), "velocityPolarity")),
                ["instant_heal"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Instant heal", "", null), byte.MaxValue, new EffectProperty(States.ChangeHealth(Level * 5), "")),
                ["instant_damage"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Instant damage", "", null), byte.MaxValue, new EffectProperty(States.ChangeHealth(Level * -5), "")),
                ["regeneration"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Regeneration", "Restores " + 5 + " health every " + 5/Level +" seconds.",  ColorizeRoman(Level, EffectColor.Positive)), byte.MaxValue, new EffectProperty(States.ChangeHealth(5, 5/Level), "regeneration")),
                ["regeneration%"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Regeneration", "Restores " + 5 + "% health every " + 5/Level +" seconds.",  ColorizeRoman(Level, EffectColor.Positive)), byte.MaxValue, new EffectProperty(States.ChangeHealth(5f, 5/Level), "regeneration")),
                ["decay"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Decay", "Loses " + 2 + " health every " + 5/Level +" seconds.",  ColorizeRoman(Level, EffectColor.Negative)), byte.MaxValue, new EffectProperty(States.ChangeHealth(-2, (5/Level)), "regeneration")),
                ["decay%"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Decay", "Loses " + 5 + "% health every " + 5/Level +" seconds.",  ColorizeRoman(Level, EffectColor.Negative)), byte.MaxValue, new EffectProperty(States.ChangeHealth(-0.05f, (5/Level)), "regeneration"))
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
    public EffectTemplate(PayWorld.EffectController.EffectDisplay display, byte maxLevel, params EffectProperty[] properties)
    {
        Properties = properties;
        TemplateDisplay = display;
        MaxLevel = maxLevel;
    }
    public byte MaxLevel;
    public PayWorld.EffectController.EffectDisplay TemplateDisplay;
    public EffectProperty[] Properties;
}