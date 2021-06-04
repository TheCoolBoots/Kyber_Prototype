using UnityEngine;
public class Grid : MonoBehaviour
{
    [SerializeField]
    private float size = 1f;

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3(
            (float)xCount * size,
            (float)yCount * size,
            (float)zCount * size);

        result += transform.position;

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        float xTemp = -1;
        for (float x = 0; x < 3; x += size)
        {
            for (float y = 0; y < 3; y += size)
            {
                var point = GetNearestPointOnGrid(new Vector3(xTemp, y + 1, 4.5f));
                Gizmos.DrawSphere(point, 0.1f);
            }
            xTemp++;

        }
    }
}