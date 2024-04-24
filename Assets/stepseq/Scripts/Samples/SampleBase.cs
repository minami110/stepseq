using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    public abstract class SampleBase : MonoBehaviour
    {
        private SampleSlot? _assignedSlot;
        
        /// <summary>
        /// Gets the price of the sample.
        /// </summary>
        public int Price
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetPrice();
        }
        
        public string HintText
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetHintText();
        }
        
        internal bool TryBuy()
        {
            // ToDo: 所持金のチェック (現在は未実装)
            
            var success = false;
            var results = ArrayPool<Collider>.Shared.Rent(16);
            var count = Physics.OverlapSphereNonAlloc(transform.position, 0.1f, results,
                1 << LayerMask.NameToLayer("Default"),
                QueryTriggerInteraction.Collide);
            var resultsAsSpan = results.AsSpan(0, count);
            foreach (var col in resultsAsSpan)
            {
                if (col.TryGetComponent(out SampleSlot slot))
                {
                    if (slot.AssignSample(this))
                    {
                        // ToDo: 所持金の現象 (現在は未実装)
                        success = true;
                        break;
                    }
                }
            }
            
            ArrayPool<Collider>.Shared.Return(results);
            return success;
        }
        
        internal void OnAssignedSampleSlot(SampleSlot slot)
        {
            if (_assignedSlot != null)
            {
                _assignedSlot.ReleaseMe(this);
            }
            
            _assignedSlot = slot;
            transform.position = _assignedSlot.transform.position;
        }
        
        protected abstract int GetPrice();
        
        protected abstract string GetHintText();
        
        public abstract void Execute(IEntity from, IEntity to);
    }
}