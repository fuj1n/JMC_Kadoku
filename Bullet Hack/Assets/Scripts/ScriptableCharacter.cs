using UnityEngine;

public class ScriptableCharacter : MonoBehaviour {
    public int X
    {
        get
        {
            return pos.x;
        }
        set
        {
            value = Mathf.Clamp(value, 0, gridSize.x - 1);
            int diff = value - pos.x;
            pos.x = value;

            transform.position += Vector3.right * diff * coordOffset;
        }
    }

    public int Y
    {
        get
        {
            return pos.y;
        }
        set
        {
            value = Mathf.Clamp(value, 0, gridSize.y - 1);
            int diff = pos.y - value;
            pos.y = value;

            transform.position += Vector3.forward * diff * coordOffset;
        }
    }

    public float coordOffset = 1F;

    public Vector2Int gridSize = new Vector2Int(3, 3);
    [SerializeField]
    private Vector2Int pos = new Vector2Int(1,1);

    private void OnDrawGizmos()
    {
        if (UnityEditor.EditorApplication.isPlaying)
            return;

        Vector3 startPos = new Vector3(transform.position.x - coordOffset * pos.x, transform.position.y, transform.position.z - coordOffset * pos.y);

        Gizmos.color = Color.magenta;

        for(int x = 0; x < gridSize.x; x++)
        {
            for(int z = 0; z < gridSize.y; z++)
            {
                Gizmos.DrawWireCube(startPos + Vector3.right * x * coordOffset + Vector3.forward * z * coordOffset, Vector3.one);
            }
        }
    }
}
