using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeScene : MonoBehaviour {

    public void LoadAllShadows()
    {
        SceneManager.LoadScene("ShadowsWithContrast", LoadSceneMode.Single);
    }

    public void LoadSingleShadow()
    {
        SceneManager.LoadScene("SingleShadow", LoadSceneMode.Single);
    }
}
