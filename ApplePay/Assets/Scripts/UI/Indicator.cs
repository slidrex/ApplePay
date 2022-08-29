using UnityEngine;
namespace Pay.UI
{

[CreateAssetMenu(menuName = "Indicators/New Indicator")]
public class Indicator : ScriptableObject
{
    public Sprite sprite;
    public UnityEngine.UI.Image.FillMethod fillMethod;
}

}