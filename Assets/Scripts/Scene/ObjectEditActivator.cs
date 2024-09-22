using Interfaces;
using UnityEngine;

namespace Scene
{
    public class ObjectEditActivator : MonoBehaviour, IEditListener
    {
        public void Edit()
        {
            gameObject.SetActive(true);
        }
    }
}
