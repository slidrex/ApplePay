using UnityEngine;
using System.Linq;

public class NavigatorMap : MonoBehaviour
{
    public UnityEngine.UI.Image mapBackground;
    public Transform elementHolder;
    public UnityEngine.UI.Image roomElement;
    private Vector2Int gridSize;
    private Vector2 cellSize;
    private Vector2 initialOffset;
    public RoomSpawner roomSpawner;
    private void Start()
    {
        gridSize = roomSpawner.GridSize;
        RenderMap();
    }
    public void RenderMap()
    {
        Vector2[] renderPositions = roomSpawner.FilledCells.Keys.ToArray();
        for(int i = 0; i < renderPositions.Length; i++)
        {
            Vector2 elementPosition = renderPositions[i]; 
            RenderElement(elementPosition);
        }
    }
    public void RenderElement(Vector2 mapPosition)
    {
        cellSize = mapBackground.rectTransform.rect.width / gridSize.x * Vector2.one;
        var obj = Instantiate(roomElement, elementHolder.transform.position, Quaternion.identity, elementHolder);
        obj.transform.localPosition = initialOffset + cellSize * mapPosition;
        obj.rectTransform.sizeDelta = cellSize - Vector2.one * 5;
        obj.transform.SetParent(elementHolder);
        obj.transform.localScale = Vector3.one;
    }
}
