using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void SetGameMode(int bugType)
    {
        GameManager.Instance.SetNextBugTypeToSpawn((BugType) bugType); 
        //doesn't show up in unity event thing unless I do it like this and I don't wanna make 20 functions that do the same thing
    }
}
