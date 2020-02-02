
using UnityEngine;
using UnityEngine.SceneManagement;

public class clickMenu : MonoBehaviour
{

    [SerializeField] private string loadLevel;

    private bool loading = false;
    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            InputAction();
        }
    }

    private void InputAction()
    {
        if (!loading)
        {
            loading = true;
            GetComponent<AudioSource>().Play();
            SceneManager.LoadScene(loadLevel);
        }
    }

   
}
