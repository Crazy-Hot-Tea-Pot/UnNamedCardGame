/// <summary>
/// Just made this to keep track of Buffs and Debuffs in the game.
/// </summary>
public static class Effects
{
    /// <summary>
    /// Type of Buffs
    /// </summary>
    public enum Buff
    {
        None,
        /// <summary>
        /// Gain shield at the end of your turn, equal to your amount of Galvanize stacks.
        /// </summary>
        Galvanize,
        /// <summary>
        /// Power attacks deal additional damage equal to your amount of power stacks.
        /// </summary>
        Power,
    }
    /// <summary>
    /// Type of Debuffs
    /// </summary>
    public enum Debuff
    {
        None,
        /// <summary>
        /// While drained, your attacks do 20% less damage.
        /// </summary>
        Drained,        
        /// <summary>
        /// While your are Jammed, you may not use Chips.
        /// </summary>
        Jam,
        /// <summary>
        /// While Worn Down, your Shield provides 30% less shield.
        /// </summary>
        WornDown
    }
    /// <summary>
    /// Type of Effects
    /// </summary>
    public enum Effect
    {
        None,
        Impervious,
        Motivation
    }

}