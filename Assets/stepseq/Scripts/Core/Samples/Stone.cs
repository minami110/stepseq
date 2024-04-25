// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    public sealed class Stone : SampleBase
    {
        private static readonly CategoryType[] _categories = { CategoryType.Nature };
        
        protected override int GetPrice()
        {
            return 1;
        }
        
        protected override string GetHintText()
        {
            return "相手に 5 ダメージ";
        }
        
        protected override CategoryType[] GetCategories()
        {
            return _categories;
        }
        
        public override void Execute(PlayerState from, PlayerState to)
        {
            var value = 5.0f * from.AttackMultiplier;
            EffectManager.AddEffect<SubHealth>(to, value);
        }
    }
}