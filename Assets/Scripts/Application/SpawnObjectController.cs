using UnityEngine;

public class SpawnObjectController : MonoBehaviour
{
    public void RegisterObj(GameObject obj)
    {
        obj.transform.parent = this.transform;
    }

    public void ClearRegisteredObj()
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}