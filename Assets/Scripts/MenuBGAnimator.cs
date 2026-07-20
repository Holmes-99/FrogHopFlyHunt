using UnityEngine;
using UnityEngine.UI;

public class MenuBGAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;  // drag your 4 images here
    [SerializeField] private float fps = 4f;   // 4 frames per second

    private Image image;
    private int currentFrame = 0;
    private float timer = 0f;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f / fps)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % frames.Length;
            image.sprite = frames[currentFrame];
        }
    }
}