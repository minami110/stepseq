using System.Runtime.CompilerServices;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider))]
    public class SampleSlot : MonoBehaviour
    {
        private SampleBase? _sample;
        
        private bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _sample == null;
        }
        
        internal void ReleaseMe(SampleBase sample)
        {
            if (_sample == sample)
            {
                _sample = null;
            }
        }
        
        public bool AssignSample(SampleBase sample)
        {
            if (!IsEmpty)
            {
                return false;
            }
            
            _sample = sample;
            _sample.OnAssignedSampleSlot(this);
            return true;
        }
        
        public bool Execute()
        {
            if (IsEmpty)
            {
                return false;
            }
            
            _sample!.Execute();
            return true;
        }
    }
}