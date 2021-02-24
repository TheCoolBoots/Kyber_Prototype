using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartesianGraph : MonoBehaviour
{
    public float xAxisMin = -10f;
    public float xAxisMax = 100f;
    public float yAxisMin = -10f;
    public float yAxisMax = 100f;

    public float dotSpriteScaleFactor = .1f;
    public float gridLineThickness = 1f;
    
    [SerializeField] private Sprite dotSprite;

    private RectTransform graphContainer;
    private RectTransform window;

    private Vector2 windowSize;

    private List<Vector2> cartesianPoints;

    private void Awake()
    {
        window = GetComponent<RectTransform>();
        windowSize = new Vector2(window.rect.width, window.rect.height);
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();

        DrawPoint(new Vector2(1f, 1f));
        DrawPoint(new Vector2(50f, 50f));

        DrawHorizontalGridLine(10);
        DrawVerticalGridLine(10);
    }

    public void SetWindow(float xMin, float xMax, float yMin, float yMax)
    {
        xAxisMin = xMin;
        xAxisMax = xMax;
        yAxisMin = yMin;
        yAxisMax = yMax;

        ReDrawCartesianPoints();
    }

    public void AddCartesianPoint(Vector2 point)
    {
        cartesianPoints.Add(point);
        DrawCartesianPoint(point);
    }

    public void ResetGraph()
    {
        foreach (Transform child in graphContainer)
        {
            Destroy(child);
        }

        // delete all old points
        cartesianPoints = new List<Vector2>();
    }

    private void ReDrawCartesianPoints()
    {
        foreach (Transform child in graphContainer)
        {
            Destroy(child);
        }

        foreach (Vector2 point in cartesianPoints)
        {
            DrawCartesianPoint(point);
        }
    }

    private void DrawCartesianPoint(Vector2 point)
    {
        if (IsPointWithinBounds(point))
        {
            float newX = Map(point.x, xAxisMin, xAxisMax, 0, windowSize.x);
            float newY = Map(point.y, yAxisMin, yAxisMax, 0, windowSize.y);
            DrawPoint(new Vector2(newX, newY));
        }
    }

    private void DrawPoint(Vector2 point)
    {
        GameObject newPoint = new GameObject("point", typeof(Image));
        newPoint.GetComponent<Image>().sprite = dotSprite;
        newPoint.transform.SetParent(graphContainer, false);

        RectTransform newPointTransform = newPoint.GetComponent<RectTransform>();
        newPointTransform.anchoredPosition = point;

        newPointTransform.anchorMin = Vector2.zero;
        newPointTransform.anchorMax = Vector2.zero;
        
        newPointTransform.sizeDelta = new Vector2(dotSpriteScaleFactor, dotSpriteScaleFactor);
    }

    private bool IsPointWithinBounds(Vector2 point)
    {
        return (point.x >= xAxisMin && point.x <= xAxisMax && point.y >= yAxisMin && point.y <= yAxisMax);
    }

    private float Map(float val, float min, float max, float outMin, float outMax)
    {
        return (outMax - outMin) * (val - min) / (max - min) + outMin;
    }

    private void DrawVerticalGridLine(float x)
    {
        GameObject newLine = new GameObject("vertGridLine", typeof(Image));
        newLine.transform.SetParent(graphContainer, false);

        RectTransform newPointTransform = newLine.GetComponent<RectTransform>();
        newPointTransform.anchoredPosition = new Vector2(x, windowSize.y / 2);

        newPointTransform.anchorMin = Vector2.zero;
        newPointTransform.anchorMax = Vector2.zero;

        newPointTransform.sizeDelta = new Vector2(gridLineThickness, windowSize.y);
    }

    private void DrawHorizontalGridLine(float y)
    {
        GameObject newLine = new GameObject("horizGridLine", typeof(Image));
        newLine.transform.SetParent(graphContainer, false);

        RectTransform newPointTransform = newLine.GetComponent<RectTransform>();
        newPointTransform.anchoredPosition = new Vector2(windowSize.x / 2, y);


        newPointTransform.anchorMin = Vector2.zero;
        newPointTransform.anchorMax = Vector2.zero;

        newPointTransform.sizeDelta = new Vector2(windowSize.x, gridLineThickness);
    }


}
