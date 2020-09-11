using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [Header("移動速度"), Range(0f, 100f)]
    public float speed = 1f;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(-speed * Time.deltaTime, 0, 0);
    }

    private void OnBecameInvisible()
    {
        transform.position += Vector3.right * 192 * 2;
    }
}
