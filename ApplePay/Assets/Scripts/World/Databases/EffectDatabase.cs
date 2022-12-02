using System.Collections.Generic;
using PayWorld.Effect;
using System.Linq;

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
                ["slowness"] = new EffectTemplate(new EffectTemplate.EffectDisplay(GetSprite("slowness"), "Slowness", "Movement speed decreased by {0,-100}%.", ColorizeRoman(Level, EffectColor.Negative), new Pay.UI.UIManager.TextField(textConfigDatabase.GetItem("lore"), "Agent pidorasik makes you slow)))")), 10, EffectType.Negative, new EffectProperty(EffectActionPresets.AttributePercent("movementSpeed", -0.1f * Level))),
                ["speed"] = new EffectTemplate(new EffectTemplate.EffectDisplay(GetSprite("slowness"), "Speed", "Movement speed increases by {0,100}%.", ColorizeRoman(Level, EffectColor.Positive)), byte.MaxValue, EffectType.Positive, new EffectProperty(EffectActionPresets.AttributePercent("movementSpeed", 0.1f * Level))),
                ["move_constraint"] = new EffectTemplate(new EffectTemplate.EffectDisplay(GetSprite("Interact.png"), "Move Constraint", "Constraints your movement abilities.", ColorizeRoman(Level, EffectColor.Negative)), 1, EffectType.Unassigned , new EffectProperty(EffectActionPresets.MoveConstraint())),
                ["speed_hack"] = new EffectTemplate(new EffectTemplate.EffectDisplay(GetSprite("Interact.png"), "Hack Haste", "Increases hack speed by {0,100}%.", ColorizeRoman(Level, EffectColor.Positive)), byte.MaxValue, EffectType.Positive, new EffectProperty(EffectActionPresets.AttributePercent("hackSpeed", 0.1f * Level))),
                ["strength"] = new EffectTemplate(new EffectTemplate.EffectDisplay(null, "Strength", "Increases attack damage by {0,100}%.", ""), byte.MaxValue, EffectType.Positive, new EffectProperty(EffectActionPresets.AttributePercent("attack_damage", Level * 0.1f))),
                ["reverse_control"] = new EffectTemplate(new EffectTemplate.EffectDisplay(null, "Reverse", "Reverses your control.", ""), 1, EffectType.Unassigned, new EffectProperty(EffectActionPresets.AttributeMultiply("movementSpeed", -1))),
                ["instant_health"] = new EffectTemplate(new EffectTemplate.EffectDisplay(null, "Instant heal", "", null), byte.MaxValue, EffectType.Positive, new EffectProperty(EffectActionPresets.ChangeHealth(Level * 5))),
                ["instant_damage"] = new EffectTemplate(new EffectTemplate.EffectDisplay(null, "Instant damage", "", null), byte.MaxValue, EffectType.Negative, new EffectProperty(EffectActionPresets.ChangeHealth(Level * -5))),
                ["regeneration"] = new EffectTemplate(new EffectTemplate.EffectDisplay(null, "Regeneration", "Restores {0,1} health every " + 5/Level +" seconds.",  ColorizeRoman(Level, EffectColor.Positive)), byte.MaxValue, EffectType.Positive, new EffectProperty(EffectActionPresets.ChangeHealth(5, 5/Level))),
                ["regeneration%"] = new EffectTemplate(new EffectTemplate.EffectDisplay(null, "Regeneration", "Restores {0,100}% health every " + 5/Level +" seconds.",  ColorizeRoman(Level, EffectColor.Positive)), byte.MaxValue, EffectType.Positive, new EffectProperty(EffectActionPresets.ChangeHealth(5f, 5/Level))),
                ["decay"] = new EffectTemplate(new EffectTemplate.EffectDisplay(null, "Decay", "Loses {0,1} health every " + 5/Level +" seconds.",  ColorizeRoman(Level, EffectColor.Negative)), byte.MaxValue, EffectType.Negative, new EffectProperty(EffectActionPresets.ChangeHealth(-2, (5/Level)))),
                ["decay%"] = new EffectTemplate(new EffectTemplate.EffectDisplay(null, "Decay", "Loses {0,100}% health every " + 5/Level +" seconds.",  ColorizeRoman(Level, EffectColor.Negative)), byte.MaxValue, EffectType.Negative, new EffectProperty(EffectActionPresets.ChangeHealth(-0.05f, (5/Level))))
            };
            return effects;
        }
    }
    
}
public struct EffectTemplate
{
    public string EntryTag;
    public struct EffectDisplay
    {
        public PayWorld.EffectController.EffectDisplay effectDisplay;
        public EffectDisplay(UnityEngine.Sprite sprite, string name, string description, string index, params Pay.UI.UIManager.TextField[] additionals)
        {
            effectDisplay = new PayWorld.EffectController.EffectDisplay(sprite, name, description, null, index, additionals);
        }
    }
    public EffectTemplate(EffectDisplay display, byte maxLevel, EffectType type, params EffectProperty[] properties)
    {
        Properties = properties;
        EntryTag = "";
        if(type == EffectType.Positive) EntryTag = "positiveEffect";
        else if(type == EffectType.Negative) EntryTag = "negativeEffect";
        else if(type == EffectType.Neutral) EntryTag = "neutralEffect";
        PayWorld.EffectController.EffectDisplay effectDisplay = display.effectDisplay;
        effectDisplay.EffectFormatValues = properties.Select(x => x.EffectAction).ToArray();
        TemplateDisplay = effectDisplay;
        MaxLevel = maxLevel;
    }
    public byte MaxLevel;
    public PayWorld.EffectController.EffectDisplay TemplateDisplay;
    public EffectProperty[] Properties;
}