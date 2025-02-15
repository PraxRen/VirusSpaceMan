using UnityEngine;

public class SavingSystemDEBUG : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            SavingSystem.Load();
            return;
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            SavingSystem.Save();
        }
    }
}