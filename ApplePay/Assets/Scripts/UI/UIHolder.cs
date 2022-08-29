using UnityEngine;
namespace Pay.UI
{
    public class UIHolder : MonoBehaviour
    {
        public Canvas FollowCanvas;
        public Canvas HUDCanvas;
        public UnityEngine.UI.Text TextObject;
        public System.Collections.Generic.Dictionary<byte, UIObject> InstantiatedUI = new System.Collections.Generic.Dictionary<byte, UIObject>();
        public System.Collections.Generic.Dictionary<byte, byte[]> BundleBuffer = new System.Collections.Generic.Dictionary<byte, byte[]>();
        private void Update() => TextUpdateHandler();
        private void TextUpdateHandler()
        {
            
        }
    }
}