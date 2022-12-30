using System.Text.RegularExpressions;

[System.Serializable]
public struct WeaponDisplay
{
    public ItemRarity Rarity;
    public UnityEngine.Sprite Icon;
    public ItemDescription Description;
}
[System.Serializable]
public struct CharmDisplay
{
    public ItemRarity Rarity;
    public UnityEngine.Sprite Icon;
    public ItemDescription Description;
    public CharmAddtionalField[] AdditionalFields;
    [System.Serializable]
    public struct CharmAddtionalField
    {  
        [UnityEngine.SerializeField] internal CharmEffectColor Color;
        public UnityEngine.Color GetColor()
        {
            string hex = "";
            UnityEngine.Color color;
            switch(Color)
            {
                case CharmEffectColor.Yellow:
                hex = "FFFE8D";
                break;
                case CharmEffectColor.Green:
                hex = "8DFF92";
                break;
                case CharmEffectColor.Red:
                hex = "FF8D94";
                break;
                case CharmEffectColor.Gray:
                hex = "A9A9A9";
                break;
            }
            UnityEngine.ColorUtility.TryParseHtmlString("#" + hex, out color);
            return color;
        }
        public string Text;
        internal enum CharmEffectColor
        {
            Yellow,
            Green,
            Red,
            Gray
        }
    }
    public static string FormatCharmField(string input, Charm charm)
    {
        Regex res = new Regex("{(.),(.)}");
        MatchCollection collection = res.Matches(input);
        foreach(Match match in collection)
        {
            int index = input.IndexOf(match.Value);
            input = input.Remove(index, 5);
            GroupCollection group = match.Groups;
                    float multiplier = 1f;
                    int arr_index;
                if(group[1].Value == "0")
                {
                    arr_index = int.Parse(group[2].Value);
                    AdditionalItemAttributes attribute = charm.Attributes[arr_index];
                    float value = charm.attributeFields[arr_index].GetValue();

                    if(attribute.DisplayType == Pay.Functions.Math.NumberType.Percent) multiplier *= 100;
                    input = input.Insert(index, (value * multiplier).ToString());
                }
                else if(group[1].Value == "1")
                {
                    arr_index = int.Parse(group[2].Value);
                    Charm.CharmField attribute = charm.CharmFields[arr_index];
                    float value = charm.additionalFields[charm.CharmFields[arr_index].name].GetValue();
                    if(attribute.NumberType == Pay.Functions.Math.NumberType.Percent) multiplier *= 100;
                    
                    input = input.Insert(index, (value * multiplier).ToString());
                }
        }
        return input;
    }
}
[System.Serializable]
public struct ItemDescription
{
    public string Name;
    [UnityEngine.TextArea] public string Description;

}
public enum ItemRarity
{
    Ordinary,
    ExtraOrdinary,
    Mythical
}
public static class ItemRarityExtension
{
    public static System.Collections.Generic.Dictionary<ItemRarity, ItemRarityInfo> Rarity = new System.Collections.Generic.Dictionary<ItemRarity, ItemRarityInfo>
    {
        [ItemRarity.Ordinary] = new ItemRarityInfo(ItemRarity.Ordinary, new UnityEngine.Color(0.8f, 1.0f, 0.8f), "Ordinary"),
        [ItemRarity.ExtraOrdinary] = new ItemRarityInfo(ItemRarity.Ordinary, new UnityEngine.Color(0.5f, 0f, 0.5f), "ExtraOrdinary"),
        [ItemRarity.Mythical] = new ItemRarityInfo(ItemRarity.Ordinary, UnityEngine.Color.red, "Mythical")
    };

    public static ItemRarityInfo GetRarityInfo(ItemRarity rarity) 
    {
        if(Rarity.TryGetValue(rarity, out ItemRarityInfo info))
        {
            return info;
        }
        return default(ItemRarityInfo);
    }
}
public struct ItemRarityInfo
{
    public ItemRarity rarity;
    public UnityEngine.Color color;
    public string name;
    public ItemRarityInfo(ItemRarity rarity, UnityEngine.Color color, string name)
    {
        this.name = name;
        this.rarity = rarity;
        this.color = color;
    }
    public string GetRichTextRarity()
    {
        return "<" + "color=" + "#" + UnityEngine.ColorUtility.ToHtmlStringRGBA(color) + ">" + name + "<" + "/color" + ">";
    }
}