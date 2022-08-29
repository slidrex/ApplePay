using System.Collections.Generic;
using PayWorld.Effect;
public class EffectDatabase
{
    public byte Level;
    public const string EffectIconPath = "Assets/Sprites/EffectIcons/";
    private float sourceAmount;
    public EffectDatabase(byte level)
    {
        Level = level;
    }
    private Dictionary<string, EffectTemplate> effects;
    public Dictionary<string, EffectTemplate> Effects 
    {
    
        get
        {
            effects = new Dictionary<string, EffectTemplate>
            {
                ["slowness"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay((UnityEngine.Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(EffectIconPath + "Slowness.png", typeof(UnityEngine.Sprite)), "Slowness", "Movement speed decreased by " + (Level * 0.1f) * 100 + "%.", Pay.Functions.Generic.RomanConverter(Level)), 10, States.VelocityChanger(-0.1f * Level)),
                ["speed"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay((UnityEngine.Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(EffectIconPath + "Slowness.png", typeof(UnityEngine.Sprite)), "Speed", "Movement speed increases by " + (Level * 0.1f) * 100 + "%.", Pay.Functions.Generic.RomanConverter(Level)), byte.MaxValue, States.VelocityChanger(0.1f * Level)),
                ["move_constraint"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay((UnityEngine.Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(EffectIconPath + "Interact.png", typeof(UnityEngine.Sprite)), "Move Constraint", "Constraints your movement abilities.", Pay.Functions.Generic.RomanConverter(Level)), 1, States.MoveConstraint()),
                ["speed_hack"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay((UnityEngine.Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(EffectIconPath + "Interact.png", typeof(UnityEngine.Sprite)), "Hack Haste", "Increases hack speed by "+ (Level * 0.1f) * 100 + "%.", Pay.Functions.Generic.RomanConverter(Level)), byte.MaxValue, States.HoldingHackSpeedChanger(0.1f * Level)),
                ["strength"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Strength", "Increases attack damage.", ""), byte.MaxValue, States.Strength(Level * 0.1f)),
                ["reverse_control"] = new EffectTemplate(new PayWorld.EffectController.EffectDisplay(null, "Reverse", "Reverses your control.", ""), 1, States.VelocityReverser())
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
public class EffectTemplate
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
