using UnityEngine;
using System.Linq;

public class NavigatorMap : MonoBehaviour
{
    public UnityEngine.UI.Image mapBackground;
    [SerializeField] private Transform mapElementsHolder;
    [SerializeField] private float mapElementsSize = 1.0f;
    [SerializeField] private float pathWidth;
    [SerializeField] private Transform pathContainer;
    [SerializeField] private Color roomElementColor;
    [SerializeField] private Color pathElementColor;
    private Vector2Int gridSize;
    private Vector2 cellSize;
    public RoomSpawner RoomSpawner;
    private System.Collections.Generic.Dictionary<Vector2, NavigatorElement> renderedElements = new System.Collections.Generic.Dictionary<Vector2, NavigatorElement>();
    private System.Collections.Generic.Dictionary<Vector2, UnityEngine.UI.Image> renderedPaths = new System.Collections.Generic.Dictionary<Vector2, UnityEngine.UI.Image>();
    private void Start()
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
        NavigatorElement element = new NavigatorElement(this);
        if(renderedElements.ContainsKey(mapPosition) == false)
        {
            var obj = InstantiateEmptyImage("roomElement", mapElementsHolder);
            obj.color = roomElementColor;
            obj.transform.localPosition = cellSize * mapPosition;
            obj.rectTransform.sizeDelta = cellSize * mapElementsSize;
            element.position = mapPosition;
            element.element = obj;
            
            renderedElements.Add(mapPosition, element);
        }
        else return renderedElements[mapPosition];
        return element;
    }
    public void RenderPath(Vector2 first, Vector2 second)
    {
        Vector2 pathKey = new Vector4(first.x + second.x, second.y + second.y);

        Vector2 firstPosition = renderedElements[first].element.transform.localPosition;
        Vector2 secondPosition = renderedElements[second].element.transform.localPosition;
        
        if(renderedPaths.ContainsKey(pathKey) == false)
        {
            Vector2 distance = (secondPosition - firstPosition);
            UnityEngine.UI.Image path = InstantiateEmptyImage("NavPath", pathContainer);
            path.color = pathElementColor;
            path.transform.localPosition = Vector2.Lerp(firstPosition, secondPosition,  0.5f);
            
            Vector2 sizeDelta =  Mathf.Abs(distance.x) > Mathf.Abs(distance.y) ? new Vector2(Mathf.Abs(distance.x), cellSize.y * pathWidth): new Vector2(cellSize.x * pathWidth, Mathf.Abs(distance.y));
            path.rectTransform.sizeDelta = sizeDelta;
            
            path.transform.localScale = Vector2.one;
            renderedPaths.Add(pathKey, path);
        }
    }
    internal UnityEngine.UI.Image InstantiateEmptyImage(string name, Transform parent)
    {
        var image = new GameObject(name).AddComponent(typeof(UnityEngine.UI.Image)) as UnityEngine.UI.Image;
        image.transform.SetParent(parent);
        image.transform.localScale = Vector3.one;
        return image;
    }
    public class NavigatorElement
    {
        private NavigatorMap attachedMap;
        public UnityEngine.UI.Image element;
        public Vector2 position;
        public NavigatorElement(NavigatorMap map)
        {
            this.position = Vector2.zero;
            element = null;
            attachedMap = map;
        }
        public GameObject PushImage(Sprite image, Vector2 localScale)
        {
            UnityEngine.UI.Image img = attachedMap.InstantiateEmptyImage("NavElementImage", element.transform);
            
            img.transform.localPosition = Vector3.zero;
            img.transform.localScale = localScale;
            img.rectTransform.sizeDelta = element.rectTransform.sizeDelta;
            
            img.sprite = image;
            return img.gameObject;
        }
    }
}
