using UnityEngine;

public class ArrowUIManager : MonoBehaviour
{
    public static ArrowUIManager Instance;

    [Header("UI")]
    public GameObject arrowIndicator;
    public float autoHideTime = 2f;

    private void Awake()
    {
        Instance = this;
        if (arrowIndicator) arrowIndicator.SetActive(false);
    }

    public void ShowArrow()
    {
        if (!arrowIndicator) return;

        arrowIndicator.SetActive(true);
        CancelInvoke(nameof(HideArrow));
        Invoke(nameof(HideArrow), autoHideTime);
    }

    private void HideArrow()
    {
        if (!arrowIndicator) return;
        arrowIndicator.SetActive(false);
    }
}
