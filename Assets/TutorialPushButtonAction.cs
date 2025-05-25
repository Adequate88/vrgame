using UnityEngine;

public class TutorialPushButtonAction : MonoBehaviour
{
    public GameObject table;

    public void onChangeTableColor()
    {
        table.GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void exitChangeTableColor()
    {
        table.GetComponent<Renderer>().material.color = Color.white;
    }
}
