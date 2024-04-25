using NUnit.Framework;
using stepseq;

public class EntityStateTests
{
    [Test]
    public void Health00()
    {
        using var player0 = new EntityState();
        
        // 体力を初期化
        player0.AddStack(StackType.AddHealth, 100f);
        player0.AddStack(StackType.AddMaxHealth, 100f);
        player0.Solve(0);
        
        // ダメージを与える
        player0.AddStack(StackType.SubHealth, 10f);
        player0.Solve(0);
        Assert.That(player0.Health.CurrentValue, Is.EqualTo(90f));
    }
    
    [Test]
    public void MaxHealth00()
    {
        using var player0 = new EntityState();
        
        // 体力を初期化
        player0.AddStack(StackType.AddHealth, 100f);
        player0.AddStack(StackType.AddMaxHealth, 100f);
        player0.Solve(0);
        
        // 最大体力を 20 増やして 10 回復する
        player0.AddStack(StackType.AddMaxHealth, 20f);
        player0.AddStack(StackType.AddHealth, 10f);
        player0.Solve(0);
        Assert.That(player0.Health.CurrentValue, Is.EqualTo(110f));
        
        // 体力を 20 回復しても, 最大体力以上にはならない
        player0.AddStack(StackType.AddHealth, 20f);
        player0.Solve(0);
        Assert.That(player0.Health.CurrentValue, Is.EqualTo(120f));
    }
    
    [Test]
    public void Shield00()
    {
        using var player0 = new EntityState();
        
        // 体力を初期化
        player0.AddStack(StackType.AddHealth, 100f);
        player0.AddStack(StackType.AddMaxHealth, 100f);
        player0.Solve(0);
        
        // シールドを追加してダメージを与える
        player0.AddStack(StackType.AddShield, 20f);
        player0.AddStack(StackType.SubHealth, 10f);
        player0.Solve(0);
        Assert.That(player0.Shield.CurrentValue, Is.EqualTo(10f));
        Assert.That(player0.Health.CurrentValue, Is.EqualTo(100f));
        
        // シールドを超えるダメージを与える
        player0.AddStack(StackType.SubHealth, 20f);
        player0.Solve(0);
        Assert.That(player0.Shield.CurrentValue, Is.EqualTo(0f));
        Assert.That(player0.Health.CurrentValue, Is.EqualTo(90f));
    }
}