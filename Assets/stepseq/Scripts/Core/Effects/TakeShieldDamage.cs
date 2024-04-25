// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    public sealed class TakeShieldDamage : EffectBase
    {
        internal override void Execute(IEntity target, object arg0)
        {
            target.AddStack(StackType.SubShield, (float)arg0);
        }
    }
}