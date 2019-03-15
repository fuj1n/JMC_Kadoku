using System.Collections.Generic;
using UnityEngine;

namespace BulletHack.Scripting
{
    public class PowerupHost : MonoBehaviour
    {
        public ScriptableCharacter character;
        public GameObject[] powerupTypes = { };

        public Vector2Int powerupCount;
        public Vector2Int startPos = Vector2Int.one;

        [ColorUsage(false)]
        public Color gizmo;

        public void CreatePowerups()
        {
            foreach (Transform t in transform)
                Destroy(t.gameObject);

            if (powerupTypes.Length <= 0)
                return;

            List<Vector2Int> taken = new List<Vector2Int>();

            int count = Random.Range(powerupCount.x, powerupCount.y + 1);

            for (int i = 0; i < count; i++)
            {
                GameObject powerup = Instantiate(powerupTypes[Random.Range(0, powerupTypes.Length)], transform);

                int limit = 10;

                Vector2Int pos;
                do
                {
                    pos = new Vector2Int(
                            Random.Range(0, character.gridSize.x),
                            Random.Range(0, character.gridSize.y)
                    );

                    limit--;
                    if (limit <= 0)
                        return;
                } while (taken.Contains(pos));

                taken.Add(pos);

                while (pos.x > startPos.x)
                {
                    pos.x--;
                    powerup.transform.Translate(Vector3.left * character.coordOffset);
                }

                while (pos.x < startPos.y)
                {
                    pos.x++;
                    powerup.transform.Translate(Vector3.right * character.coordOffset);
                }

                while (pos.y > startPos.y)
                {
                    pos.y--;
                    powerup.transform.Translate(Vector3.back * character.coordOffset);
                }

                while (pos.y < startPos.y)
                {
                    pos.y++;
                    powerup.transform.Translate(Vector3.forward * character.coordOffset);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
                return;

            Vector3 startPos = new Vector3(transform.position.x - character.coordOffset * this.startPos.x, transform.position.y, transform.position.z - character.coordOffset * this.startPos.y);

            gizmo.a = 1;
            Gizmos.color = gizmo;

            for (int x = 0; x < character.gridSize.x; x++)
            {
                for (int z = 0; z < character.gridSize.y; z++)
                {
                    Gizmos.DrawWireCube(startPos + Vector3.right * x * character.coordOffset + Vector3.forward * z * character.coordOffset, Vector3.one);
                }
            }
        }
    }
}