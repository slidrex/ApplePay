using UnityEngine;
namespace Pay.UI
{
    public class UIHolder : MonoBehaviour
    {
        public Canvas FollowCanvas;
        public Canvas HUDCanvas;
        public UnityEngine.UI.Text TextObject;
        public System.Collections.Generic.List<UIElement> InstantiatedUI = new System.Collections.Generic.List<UIElement>();
        private void Update() => TextUpdateHandler();
        private void TextUpdateHandler()
        {
            
        }
    }
}