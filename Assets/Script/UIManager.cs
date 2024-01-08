using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button UndoButton;
    public GameObject WinCanvas;

    private void Start()
    {
        UndoButtonView(false);
    }
    public void UndoButtonView(bool active)
    {
        UndoButton.gameObject.SetActive(active);
    }

    public void ButtonUndo()
    {
        GameManager.instance.gridManager.RestoreTiles();
    }
    public void ButtonRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UIWin()
    {
        WinCanvas.SetActive(true);
    }
}
