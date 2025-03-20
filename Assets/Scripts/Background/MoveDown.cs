using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public float time = 5f;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float elapsedTime = 0f;
    private bool shouldMove = false;

    public void StartMoving()
    {
        startPos = transform.position;
        targetPos = new Vector3(startPos.x, -27f, startPos.z);
        elapsedTime = 0f;
        shouldMove = true;
    }

    void Update()
    {
        if (shouldMove)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsedTime / time));
            transform.position = Vector3.Lerp(startPos, targetPos, t);
        }
    }
}
