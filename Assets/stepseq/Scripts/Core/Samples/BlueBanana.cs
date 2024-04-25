// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    public sealed class BlueBanana : SampleBase
    {
        private static readonly CategoryType[] _categories = { CategoryType.Nature };
        
        protected override int GetPrice()
        {
            return 10;
        }
        
        protected override string GetHintText()
        {
            return "相手のシールド値だけ体力を回復";
        }
        
        protected override CategoryType[] GetCategories()
        {
            return _categories;
        }
        
        public override void Execute(EntityState from, EntityState to)
        {
            // 相手のシールド値を参照する
            var value = to.Shield.CurrentValue;
            if (value <= 0)
            {
                return;
            }
            
            // 相手のシールド値だけ体力を回復する
            EffectManager.AddEffect<AddHealth>(from, value);
        }
    }
}