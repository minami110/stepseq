// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    public sealed class TakeHealthDamage : EffectBase
    {
        internal override void Execute(IEntity target, object arg0)
        {
            target.AddStack(StackType.HealthDamage, (float)arg0);
        }
    }
}