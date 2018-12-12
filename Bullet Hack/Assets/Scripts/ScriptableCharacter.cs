using DG.Tweening;
using UnityEngine;

public class ScriptableCharacter : MonoBehaviour
{
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

            Move(Vector3.right * diff * coordOffset);
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

            Move(Vector3.forward * diff * coordOffset);
        }
    }

    public int health = 3;

    public float rotateIntensity = 2F;

    [ColorUsage(false)]
    public Color gizmo;

    public float coordOffset = 1F;

    [HideInInspector]
    public float tweenSpeed;

    public Vector2Int gridSize = new Vector2Int(3, 3);
    [SerializeField]
    private Vector2Int pos = new Vector2Int(1, 1);

    public GameObject bullet;

    private void Move(Vector3 val)
    {
        Vector3 direction = val / val.magnitude;

        Sequence move = DOTween.Sequence();

        move.Append(transform.DOBlendableLocalRotateBy(-direction * rotateIntensity, tweenSpeed * .2F));
        move.AppendInterval(tweenSpeed * .6F);
        move.Append(transform.DORotateQuaternion(transform.rotation, tweenSpeed * .2F));

        transform.DOMove(transform.position + val, tweenSpeed).SetEase(Ease.InOutSine);

        move.Play();
    }

    private void Update()
    {
        if (health <= 0)
            Destroy(gameObject);
    }

    public void Shoot()
    {
        GameObject b = Instantiate(bullet);
        b.transform.position = transform.position;
        b.transform.forward = transform.forward;

        Bullet bObj = b.GetComponent<Bullet>();
        if (bObj)
            bObj.target = CombatManager.Instance.Script.OtherAvatar.transform;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
            return;

        Vector3 startPos = new Vector3(transform.position.x - coordOffset * pos.x, transform.position.y, transform.position.z - coordOffset * pos.y);

        gizmo.a = 1;
        Gizmos.color = gizmo;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int z = 0; z < gridSize.y; z++)
            {
                Gizmos.DrawWireCube(startPos + Vector3.right * x * coordOffset + Vector3.forward * z * coordOffset, Vector3.one);
            }
        }
    }
}
