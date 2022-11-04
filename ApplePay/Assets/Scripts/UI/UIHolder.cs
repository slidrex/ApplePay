using System.Linq;
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
            UIElement[] textObjects = InstantiatedUI.FindAll(x => x.GetType() == typeof(TextObject)).ToArray();
            for(int i = 0; i < textObjects.Length; i++)
            {
                TextObject current = (TextObject)textObjects[i];
                if(current.curDuration < current.Duration)
                {
                    current.curDuration += Time.deltaTime;
                    current.Text.color = new Color(current.Text.color.r, current.Text.color.g, current.Text.color.b, current.AlphaBehaviour.Evaluate(current.curDuration/current.Duration));
                    textObjects[i] = current;
                }
                else
                {
                    current.OnRemove();
                    
                }
            }
        }
    }
}