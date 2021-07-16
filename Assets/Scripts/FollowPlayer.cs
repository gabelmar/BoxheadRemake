using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private Vector3 relativePosToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        relativePosToPlayer = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + relativePosToPlayer;
    }
}
