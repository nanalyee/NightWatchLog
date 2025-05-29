using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("Target Scene Settings")]
    [SerializeField] private string sceneName;

    private void Awake()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(LoadTargetScene);
        }
        else
        {
            Debug.LogWarning($"[SceneLoader] Button component not found on {gameObject.name}");
        }
    }

    private void LoadTargetScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("[SceneLoader] Scene name is not set!");
            return;
        }

        Debug.Log($"[SceneLoader] Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}
