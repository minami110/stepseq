﻿// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    public sealed class HandGun : SampleBase
    {
        protected override int GetPrice()
        {
            return 5;
        }
        
        protected override string GetHintText()
        {
            return "10 ダメージを与える";
        }
        
        public override void Execute(IEntity from, IEntity to)
        {
            // 相手にダメージ 10 を与える
            EffectManager.AddEffect<TakeHealthDamage>(to, 10f);
        }
    }
}