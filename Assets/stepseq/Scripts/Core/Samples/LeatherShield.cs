// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    public sealed class LeatherShield : SampleBase
    {
        // Category: None
        private static readonly CategoryType[] _categories = { };
        
        protected override int GetPrice()
        {
            return 5;
        }
        
        protected override string GetHintText()
        {
            return "10 シールドを得る";
        }
        
        protected override CategoryType[] GetCategories()
        {
            return _categories;
        }
        
        public override void Execute(PlayerState from, PlayerState to)
        {
            // 実行者にシールド 10 を与える
            EffectManager.AddEffect<TakeShield>(from, 10f);
        }
    }
}