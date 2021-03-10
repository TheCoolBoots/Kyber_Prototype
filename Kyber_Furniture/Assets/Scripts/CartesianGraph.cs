using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartesianGraph : MonoBehaviour
{
    private float xAxisMin;
    private float xAxisMax;
    private float yAxisMin;
    private float yAxisMax;

    public int xMinStepRange = -2;
    public int xMaxStepRange = 3;
    public int yMinStepRange = -4;
    public int yMaxStepRange = 3;

    public float xStep = 10;
    public float yStep = 10;

    public float dotSpriteScaleFactor = .1f;
    public float gridLineThickness = 1f;

    [SerializeField] public Color backgroundColor;
    [SerializeField] public Color gridlineColor;
    [SerializeField] public Color dotColor;
    [SerializeField] public Color originColor;

    [SerializeField] private Sprite dotSprite;

    private RectTransform graphContainer;
    private RectTransform window;

    private Vector2 windowSize;

    private List<Vector2> cartesianPoints;
    private List<GameObject> horizGridlines;
    private List<GameObject> vertGridlines;

    private GameObject xAxis;
    private GameObject yAxis;
    private GameObject origin;

    private void Awake()
    {
        window = GetComponent<RectTransform>();
        windowSize = new Vector2(window.rect.width, window.rect.height);
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        Image background = transform.Find("Background").GetComponent<Image>();
        background.color = backgroundColor;
        cartesianPoints = new List<Vector2>();

        xAxisMin = xStep * xMinStepRange;
        xAxisMax = xStep * xMaxStepRange;
        yAxisMin = yStep * yMinStepRange;
        yAxisMax = yStep * yMaxStepRange;

        InstantiateGridines();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveWindow(new Vector2(4f, 4f));
            Debug.Log("Moving Window");
        }
    }

    public void MoveWindow(Vector2 cartesianTranslation)
    {
        xAxisMin += cartesianTranslation.x;
        xAxisMax += cartesianTranslation.x;
        yAxisMin += cartesianTranslation.y;
        yAxisMax += cartesianTranslation.y;

        for (int i = 0; i < cartesianPoints.Count; i++)
        {
            cartesianPoints[i] += cartesianTranslation;
        }

        DrawCartesianPoints();

        float realTranslationY = windowSize.y * cartesianTranslation.y / (yAxisMax - yAxisMin);
        float realTranslationX = windowSize.x * cartesianTranslation.x / (xAxisMax - xAxisMin);

        MoveHorizontalGridlines(realTranslationY);
        MoveVerticalGridlines(realTranslationX);
        
/*        if(origin == null)
        {
            DrawOrigin();
        }
        if(xAxis == null)
        {
            DrawXAxis();
        }
        if(yAxis == null)
        {
            DrawYAxis();
        }*/
    }

    public void AddCartesianPoint(Vector2 point)
    {
        cartesianPoints.Add(point);
        DrawCartesianPoint(point, dotColor);
    }

    private void InstantiateGridines()
    {
        float currentVertLineX = (int)(xAxisMin/xStep) * xStep;
        float currentHorizLineY = (int)(yAxisMin/yStep) * yStep;

        horizGridlines = new List<GameObject>();
        vertGridlines = new List<GameObject>();

        if (xAxisMin > 0)
            currentVertLineX += xStep;
        if (yAxisMin > 0)
            currentHorizLineY += yStep;

        Debug.Log("startX: " + currentVertLineX);
        Debug.Log("startY: " + currentHorizLineY);

        while(currentVertLineX <= xAxisMax)
        {
            DrawVerticalGridline(Map(currentVertLineX, xAxisMin, xAxisMax, 0, windowSize.x), gridlineColor);
            currentVertLineX += xStep;
        }

        while (currentHorizLineY <= yAxisMax)
        {
            DrawHorizontalGridline(Map(currentHorizLineY, yAxisMin, yAxisMax, 0, windowSize.y), gridlineColor);
            currentHorizLineY += yStep;
        }

        DrawOrigin();
        DrawXAxis();
        DrawYAxis();
    }

    private void DrawCartesianPoints()
    {
        foreach (Vector2 point in cartesianPoints)
        {
            if(IsPointWithinCartesianBounds(point))
                DrawCartesianPoint(point, dotColor);
        }
    }

    private Vector2 DrawCartesianPoint(Vector2 point, Color color)
    {
        float newX = Map(point.x, xAxisMin, xAxisMax, 0, windowSize.x);
        float newY = Map(point.y, yAxisMin, yAxisMax, 0, windowSize.y);
        Vector2 pos = new Vector2(newX, newY);
        DrawPoint(pos, color);

        return pos;
    }

    private void DrawPoint(Vector2 point, Color color)
    {
        GameObject newPoint = new GameObject("point", typeof(Image));
        newPoint.GetComponent<Image>().sprite = dotSprite;
        newPoint.GetComponent<Image>().color = color;
        newPoint.transform.SetParent(graphContainer, false);

        RectTransform newPointTransform = newPoint.GetComponent<RectTransform>();
        newPointTransform.anchoredPosition = point;

        newPointTransform.anchorMin = Vector2.zero;
        newPointTransform.anchorMax = Vector2.zero;
        
        newPointTransform.sizeDelta = new Vector2(dotSpriteScaleFactor, dotSpriteScaleFactor);
    }

    private bool IsPointWithinCartesianBounds(Vector2 point)
    {
        return (point.x >= xAxisMin && point.x <= xAxisMax && point.y >= yAxisMin && point.y <= yAxisMax);
    }

    private float Map(float val, float min, float max, float outMin, float outMax)
    {
        return (outMax - outMin) * (val - min) / (max - min) + outMin;
    }

    private GameObject DrawVerticalGridline(float x, Color color)
    {
        GameObject newLine = new GameObject("vertGridLine", typeof(Image));
        newLine.transform.SetParent(graphContainer, false);

        newLine.GetComponent<Image>().color = color;

        RectTransform newLineTransform = newLine.GetComponent<RectTransform>();
        newLineTransform.anchoredPosition = new Vector2(x, windowSize.y / 2);

        newLineTransform.anchorMin = Vector2.zero;
        newLineTransform.anchorMax = Vector2.zero;

        newLineTransform.sizeDelta = new Vector2(gridLineThickness, windowSize.y);

        vertGridlines.Add(newLine);

        return newLine;
    }

    private GameObject DrawHorizontalGridline(float y, Color color)
    {
        GameObject newLine = new GameObject("horizGridLine", typeof(Image));
        newLine.transform.SetParent(graphContainer, false);

        newLine.GetComponent<Image>().color = color;

        RectTransform newLineTransform = newLine.GetComponent<RectTransform>();
        newLineTransform.anchoredPosition = new Vector2(windowSize.x / 2, y);

        newLineTransform.anchorMin = Vector2.zero;
        newLineTransform.anchorMax = Vector2.zero;

        newLineTransform.sizeDelta = new Vector2(windowSize.x, gridLineThickness);

        horizGridlines.Add(newLine);

        return newLine;
    }

    private void MoveHorizontalGridlines(float translateY)
    {
        foreach(GameObject line in horizGridlines)
        {
            RectTransform linePos = line.GetComponent<RectTransform>();

            if (linePos.anchoredPosition.y + translateY > windowSize.y)
            {
                linePos.anchoredPosition += new Vector2(0, translateY - windowSize.y);
            }
            else if (linePos.anchoredPosition.y + translateY < 0)
            {
                linePos.anchoredPosition += new Vector2(0, translateY + windowSize.y);
            }
            else
            {
                linePos.anchoredPosition += new Vector2(0, translateY);
            }
        }
    }

    private void MoveVerticalGridlines(float translateX)
    {
        foreach (GameObject line in vertGridlines)
        {
            RectTransform linePos = line.GetComponent<RectTransform>();

            if (linePos.anchoredPosition.x + translateX > windowSize.x)
            {
                linePos.anchoredPosition += new Vector2(translateX - windowSize.x, 0);
            }
            else if (linePos.anchoredPosition.x + translateX < 0)
            {
                linePos.anchoredPosition += new Vector2(translateX + windowSize.x, 0);
            }
            else
            {
                linePos.anchoredPosition += new Vector2(translateX, 0);
            }
        }
    }

    private void DrawOrigin()
    {
        if (IsPointWithinCartesianBounds(Vector2.zero))
        {
            cartesianPoints.Add(DrawCartesianPoint(Vector2.zero, originColor));
        }
    }

    private void DrawXAxis()
    {
        if (Mathf.Clamp(0, yAxisMin, yAxisMax) == 0)
        {
            Debug.Log("drawing xAxis");
            horizGridlines.Add(DrawHorizontalGridline(Map(0, yAxisMin, yAxisMax, 0, windowSize.y), originColor));
        }
    }

    private void DrawYAxis()
    {
        if (Mathf.Clamp(0, xAxisMin, xAxisMax) == 0)
        {
            vertGridlines.Add(DrawVerticalGridline(Map(0, xAxisMin, xAxisMax, 0, windowSize.x), originColor));
        }
    }

}
