// Copyright Edanoue, Inc. All Rights Reserved.

using System;
using Edanoue.Rx;
using Random = UnityEngine.Random;

namespace stepseq
{
    public sealed class HandGun : SampleBase
    {
        private const           int            _MAX_COUNT  = 8;
        private static readonly CategoryType[] _categories = { CategoryType.Fire, CategoryType.Fly };
        private readonly        IDisposable    _eventManagerSubscription;
        private                 int            _count = _MAX_COUNT;
        
        public HandGun()
        {
            _eventManagerSubscription = EventManager.OnBattleEnd
                .Subscribe(this, (_, state) => { _count = _MAX_COUNT; });
        }
        
        protected override int GetPrice()
        {
            return 5;
        }
        
        protected override string GetHintText()
        {
            return $"弾を1消費して 10 ダメージを与える, 弾はバトルのたびに補充される.\n残り: {_count} 発";
        }
        
        protected override CategoryType[] GetCategories()
        {
            return _categories;
        }
        
        public override void Execute(PlayerState from, PlayerState to)
        {
            if (_count <= 0)
            {
                return;
            }
            
            // 相手が回避するかどうかを計算する
            var rate = to.Evasion;
            if (rate > 0.0f && Random.value < rate)
            {
                return;
            }
            
            _count--;
            // 相手にダメージ 10 を与える
            var value = 10.0f * from.AttackMultiplier;
            EffectManager.AddEffect<SubHealth>(to, value);
        }
        
        public override void Dispose()
        {
            _eventManagerSubscription.Dispose();
        }
    }
}