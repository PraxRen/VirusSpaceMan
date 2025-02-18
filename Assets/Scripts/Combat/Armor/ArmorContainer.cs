using System.Linq;
using UnityEngine;

public class ArmorContainer : MonoBehaviour
{
    [SerializeField][ReadOnly] private Armor[] _armors;

#if UNITY_EDITOR
    [ContextMenu("Find Armors")]
    private void FindArmors()
    {
        _armors = GetComponentsInChildren<Armor>(true);
    }
#endif

    public Armor GetArmor(string idArmor)
    {
        return _armors.FirstOrDefault(armor => armor.Id == idArmor);
    }
}
