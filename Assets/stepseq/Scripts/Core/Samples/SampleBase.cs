using System;
using System.Runtime.CompilerServices;

namespace stepseq
{
    public abstract class SampleBase : IDisposable
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
        
        public CategoryType[] Categories
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetCategories();
        }
        
        public virtual void Dispose()
        {
        }
        
        internal bool AssignToSlot(SampleSlot slot)
        {
            // ToDo: 購入可能かどうか所持金のチェック
            // ToDo: 所持金の減少
            return slot.AssignSample(this);
        }
        
        internal void OnAssignedSampleSlot(SampleSlot slot)
        {
            if (_assignedSlot != null)
            {
                _assignedSlot.ReleaseMe(this);
            }
            
            _assignedSlot = slot;
        }
        
        internal void Sell()
        {
            if (_assignedSlot != null)
            {
                _assignedSlot.ReleaseMe(this);
                _assignedSlot = null;
                
                // ToDo: 所持金に還元する
            }
        }
        
        protected abstract int GetPrice();
        
        protected abstract string GetHintText();
        
        protected abstract CategoryType[] GetCategories();
        
        public abstract void Execute(PlayerState from, PlayerState to);
    }
}