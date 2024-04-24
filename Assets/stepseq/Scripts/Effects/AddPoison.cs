// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    public sealed class AddPoison : EffectBase
    {
        internal override void Execute(IEntity target, object arg0)
        {
            target.AddStack(StackType.Poison, (float)arg0);
        }
    }
}