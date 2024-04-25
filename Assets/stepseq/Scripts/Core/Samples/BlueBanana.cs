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
            return "自身の体力を 5 回復";
        }
        
        protected override CategoryType[] GetCategories()
        {
            return _categories;
        }
        
        public override void Execute(PlayerState from, PlayerState to)
        {
            var value = 5.0f * from.HealMultiplier;
            EffectManager.AddEffect<AddHealth>(from, value);
        }
    }
}