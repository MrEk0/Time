using Interfaces;
using UnityEngine;

namespace Scene
{
    public class ObjectConfirmDeactivator : MonoBehaviour, IConfirmListener
    {
        public void Confirm()
        {
            gameObject.SetActive(false);
        }
    }
}
