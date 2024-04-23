using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider))]
    public class SampleSlot : MonoBehaviour
    {
        private bool IsEmpty { get; set; }
        
        private void Awake()
        {
            IsEmpty = true;
        }
        
        public bool AssignSample(Sample sample)
        {
            if (!IsEmpty)
            {
                return false;
            }
            
            sample.transform.position = transform.position;
            IsEmpty = false;
            return true;
        }
    }
}