using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Icon;

    private bool isVisible = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isVisible = !isVisible;
            Icon.SetActive(isVisible);
        }
    }
}
