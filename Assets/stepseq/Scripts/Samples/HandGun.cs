// Copyright Edanoue, Inc. All Rights Reserved.

using Edanoue.Rx;

namespace stepseq
{
    public sealed class HandGun : SampleBase
    {
        private const int _MAX_COUNT = 8;
        
        private int _count = _MAX_COUNT;
        
        private void Awake()
        {
            TimeManager.OnPlay
                .Subscribe(this, (_, state) => { _count = _MAX_COUNT; })
                .RegisterTo(destroyCancellationToken);
        }
        
        protected override int GetPrice()
        {
            return 5;
        }
        
        protected override string GetHintText()
        {
            return $"10 ダメージを与える. 残り: {_count} 発";
        }
        
        public override void Execute(IEntity from, IEntity to)
        {
            if (_count <= 0)
            {
                return;
            }
            
            _count--;
            // 相手にダメージ 10 を与える
            EffectManager.AddEffect<TakeHealthDamage>(to, 10f);
        }
    }
}