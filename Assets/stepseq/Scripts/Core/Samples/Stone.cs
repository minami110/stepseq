// Copyright Edanoue, Inc. All Rights Reserved.

using UnityEngine;

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
            // 相手が回避するかどうかを計算する
            var rate = to.Evasion;
            if (rate > 0.0f && Random.value < rate)
            {
                return;
            }
            
            var value = 5.0f * from.AttackMultiplier;
            EffectManager.AddEffect<SubHealth>(to, value);
        }
    }
}