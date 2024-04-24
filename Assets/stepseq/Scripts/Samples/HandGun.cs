// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    public sealed class HandGun : SampleBase
    {
        public override void Execute()
        {
            EffectManager.AddEffect<TakeHealthDamage>(10f);
        }
    }
}