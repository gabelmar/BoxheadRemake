using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public GameObject bloodDecalPrefab;

    private Vector3 spawnPos;
    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<HealthSystem>().OnDamageTaken += SpawnBlooodDecal;
    }
    private void OnDestroy()
    {
        GetComponent<HealthSystem>().OnDamageTaken -= SpawnBlooodDecal;
    }

    private void SpawnBlooodDecal(int obj)
    {
        spawnPos = transform.position;
        spawnPos.y = Random.Range(0.05f, 0.15f); // prevent camera z fighting
        float zRotation = Random.Range(0f, 180f);
        Transform decal = Instantiate(bloodDecalPrefab, spawnPos, Quaternion.Euler(90f, 0f, zRotation)).transform;
        Vector3 randomizedScale = decal.transform.localScale;
        randomizedScale.x *= Random.Range(1.0f, 1.5f);
        randomizedScale.y *= Random.Range(1.0f, 1.5f);
        decal.transform.localScale = randomizedScale;
    }
}
