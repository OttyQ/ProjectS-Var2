using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGrid : LayoutGroup
{
    public int rows;
    public int columns;
    public UnityEngine.Vector2 cellSize;
    public UnityEngine.Vector2 range;


    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        float sqrt = Mathf.Sqrt(transform.childCount);
        rows = Mathf.CeilToInt(sqrt);
        columns = Mathf.CeilToInt(sqrt);

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float)columns) - ((range.x / (float)columns) * 2) - (padding.left / (float)columns)
            - (padding.right/(float)columns);

        float cellHeight = (parentHeight / (float)rows) - ((range.y / (float)rows) * 2) - (padding.top / (float)rows)
            - (padding.bottom/(float)rows);

        cellSize.x = cellWidth;
        cellSize.y = cellHeight;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (range.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (range.y * rowCount) + padding.top ;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y); 
        }
    }
    public override void CalculateLayoutInputVertical()
    {
        //throw new System.NotImplementedException();
    }

    public override void SetLayoutHorizontal()
    {
        //throw new System.NotImplementedException();
    }

    public override void SetLayoutVertical()
    {
        //throw new System.NotImplementedException();
    }
}
