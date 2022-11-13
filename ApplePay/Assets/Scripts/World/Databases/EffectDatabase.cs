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
    public SpriteDatabase EffectIconDatabase = (SpriteDatabase)PayDatabase.GetDatabase("effect_icon");
    public EffectDatabase(byte level)
    {
        Level = level;
    }
    ///<summary>Gets sprite by name (name.extension). </summary>
    private UnityEngine.Sprite GetSprite(string name) => EffectIconDatabase.GetItem(name);
    private string ColorizeText(string text, string colorHex) => Pay.Functions.String.SetRichTextTag(text, "color", colorHex);
    private string ColorizeRoman(byte level, string colorHex) => ColorizeText(Pay.Functions.Generic.RomanConverter(level), colorHex);
    private Dictionary<string, EffectTemplate> effects;
    public Dictionary<string, EffectTemplate> Effects 
    {
        get
        {
            effects = new Dictionary<string, EffectTemplate>
            {
                ["slowness"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(GetSprite("slowness"), "Slowness", "Movement speed decreased by " + (Level * 0.1f) * 100 + "%.", ColorizeRoman(Level, EffectColor.Negative), new Pay.UI.UIManager.TextField(textConfigDatabase.GetItem("lore"), "Agent pidorasik makes you slow)))")), 10, EffectType.Negative, new EffectProperty(States.VelocityChanger(-0.1f * Level))),
                ["speed"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(GetSprite("slowness"), "Speed", "Movement speed increases by " + (Level * 0.1f) * 100 + "%.", ColorizeRoman(Level, EffectColor.Positive)), byte.MaxValue, EffectType.Positive, new EffectProperty(States.VelocityChanger(0.1f * Level))),
                ["move_constraint"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(GetSprite("Interact.png"), "Move Constraint", "Constraints your movement abilities.", ColorizeRoman(Level, EffectColor.Negative)), 1, EffectType.Unassigned , new EffectProperty(States.MoveConstraint())),
                ["speed_hack"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(GetSprite("Interact.png"), "Hack Haste", "Increases hack speed by "+ (Level * 0.1f) * 100 + "%.", ColorizeRoman(Level, EffectColor.Positive)), byte.MaxValue, EffectType.Positive, new EffectProperty(States.HoldingHackSpeedChanger(0.1f * Level))),
                ["strength"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Strength", "Increases attack damage.", ""), byte.MaxValue, EffectType.Positive, new EffectProperty(States.Strength(Level * 0.1f))),
                ["reverse_control"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Reverse", "Reverses your control.", ""), 1, EffectType.Unassigned, new EffectProperty(States.VelocityReverser())),
                ["instant_health"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Instant heal", "", null), byte.MaxValue, EffectType.Positive, new EffectProperty(States.ChangeHealth(Level * 5))),
                ["instant_damage"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Instant damage", "", null), byte.MaxValue, EffectType.Negative, new EffectProperty(States.ChangeHealth(Level * -5))),
                ["regeneration"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Regeneration", "Restores " + 5 + " health every " + 5/Level +" seconds.",  ColorizeRoman(Level, EffectColor.Positive)), byte.MaxValue, EffectType.Positive, new EffectProperty(States.ChangeHealth(5, 5/Level))),
                ["regeneration%"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Regeneration", "Restores " + 5 + "% health every " + 5/Level +" seconds.",  ColorizeRoman(Level, EffectColor.Positive)), byte.MaxValue, EffectType.Positive, new EffectProperty(States.ChangeHealth(5f, 5/Level))),
                ["decay"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Decay", "Loses " + 2 + " health every " + 5/Level +" seconds.",  ColorizeRoman(Level, EffectColor.Negative)), byte.MaxValue, EffectType.Negative, new EffectProperty(States.ChangeHealth(-2, (5/Level)))),
                ["decay%"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Decay", "Loses " + 5 + "% health every " + 5/Level +" seconds.",  ColorizeRoman(Level, EffectColor.Negative)), byte.MaxValue, EffectType.Negative, new EffectProperty(States.ChangeHealth(-0.05f, (5/Level))))
            };
            return effects;
        }
    }
    
}
public struct EffectTemplate
{
    public string EntryTag;
    public EffectTemplate(PayWorld.EffectController.EffectDisplay display, byte maxLevel, EffectType type, params EffectProperty[] properties)
    {
        Properties = properties;
        EntryTag = "";
        if(type == EffectType.Positive) EntryTag = "positiveEffect";
        else if(type == EffectType.Negative) EntryTag = "negativeEffect";
        else if(type == EffectType.Neutral) EntryTag = "neutralEffect";
        
        TemplateDisplay = display;
        MaxLevel = maxLevel;
    }
    public byte MaxLevel;
    public PayWorld.EffectController.EffectDisplay TemplateDisplay;
    public EffectProperty[] Properties;
}