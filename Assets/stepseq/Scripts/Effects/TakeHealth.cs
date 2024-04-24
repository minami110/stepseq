// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    public sealed class TakeHealth : EffectBase
    {
        internal override void Execute(IEntity target, object arg0)
        {
            target.AddStack(StackType.Health, (float)arg0);
        }
    }
}