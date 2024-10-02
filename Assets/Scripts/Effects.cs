/// <summary>
/// Just made this to keep track of Buffs and Debuffs in the game.
/// </summary>
public static class Effects
{
    /// <summary>
    /// A Buff effect.
    /// </summary>
    public enum Buff
    {
        None,
        Galvanize,
        Power,
    }
    /// <summary>
    /// A debuffToApply effect.
    /// </summary>
    public enum Debuff
    {
        None,
        Gunked,
        Drained,
        WornDown,
        Jam
    }
    public enum Effect
    {
        Impervious,
        Motivation
    }

}