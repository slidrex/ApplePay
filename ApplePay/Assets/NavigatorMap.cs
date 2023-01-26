using UnityEngine;
using System.Linq;

public class NavigatorMap : MonoBehaviour
{
    public UnityEngine.UI.Image mapBackground;
    [SerializeField] private Transform mapElementsHolder;
    [SerializeField] private float mapElementsSize = 1.0f;
    public UnityEngine.UI.Image roomElement;
    private Vector2Int gridSize;
    private Vector2 cellSize;
    public RoomSpawner RoomSpawner;
    private System.Collections.Generic.Dictionary<Vector2, NavigatorElement> renderedElements = new System.Collections.Generic.Dictionary<Vector2, NavigatorElement>();
    private void Awake()
    {
        gridSize = RoomSpawner.GridSize;
        cellSize = mapBackground.rectTransform.rect.size / new Vector2((float)gridSize.x, (float)gridSize.y);
    }
    public void Clear()
    {
        for(int i = 0; i < mapElementsHolder.childCount; i++)
        {
            Destroy(mapElementsHolder.GetChild(i).gameObject);
        }
    }
    public void RenderFullMap()
    {
        Vector2[] renderPositions = RoomSpawner.FilledCells.Keys.ToArray();
        for(int i = 0; i < renderPositions.Length; i++)
        {
            Vector2 elementPosition = renderPositions[i]; 
            RenderElement(elementPosition);
        }
    }
    public NavigatorElement RenderElement(Vector2 mapPosition)
    {
        NavigatorElement element = new NavigatorElement();
        if(renderedElements.ContainsKey(mapPosition) == false)
        {
            var obj = Instantiate(roomElement, mapElementsHolder.position, Quaternion.identity, mapElementsHolder);
            obj.transform.localPosition = cellSize * mapPosition;
            obj.rectTransform.sizeDelta = cellSize * mapElementsSize;
            obj.transform.localScale = Vector3.one;
            element.position = mapPosition;
            element.element = obj;
            renderedElements.Add(mapPosition, element);
        }
        else return renderedElements[mapPosition];
        return element;
    }
    public class NavigatorElement
    {
        public UnityEngine.UI.Image element;
        public Vector2 position;
        public GameObject PushImage(Sprite image, Vector2 localScale)
        {
            UnityEngine.UI.Image img = new GameObject("NavElementImage").AddComponent(typeof(UnityEngine.UI.Image)) as UnityEngine.UI.Image;
            img.transform.SetParent(element.transform);
            img.transform.localPosition = Vector3.zero;
            img.transform.localScale = localScale;
            img.rectTransform.sizeDelta = element.rectTransform.sizeDelta;
            
            img.sprite = image;
            return img.gameObject;
        }
    }
}
