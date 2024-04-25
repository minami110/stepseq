// Copyright Edanoue, Inc. All Rights Reserved.

using System;
using Edanoue.Rx;

namespace stepseq
{
    public sealed class HandGun : SampleBase
    {
        private const           int            _MAX_COUNT  = 8;
        private                 int            _count      = _MAX_COUNT;
        private static readonly CategoryType[] _categories = { CategoryType.Fire, CategoryType.Fly };
        private readonly        IDisposable    _eventManagerSubscription;
        
        public HandGun()
        {
            _eventManagerSubscription = TimeManager.OnPlay
                .Subscribe(this, (_, state) => { _count = _MAX_COUNT; });
        }
        
        protected override int GetPrice()
        {
            return 5;
        }
        
        protected override string GetHintText()
        {
            return $"10 ダメージを与える. 残り: {_count} 発\nCategory: Fly, Fire";
        }
        
        protected override CategoryType[] GetCategories()
        {
            return _categories;
        }
        
        public override void Execute(EntityState from, EntityState to)
        {
            if (_count <= 0)
            {
                return;
            }
            
            _count--;
            // 相手にダメージ 10 を与える
            EffectManager.AddEffect<TakeHealthDamage>(to, 10f);
        }
        
        public override void Dispose()
        {
            _eventManagerSubscription.Dispose();
        }
    }
}