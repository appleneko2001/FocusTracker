namespace FocusTracker.Native
{
    /// <summary>
    /// Stripped index of windows message kind. Used for execute some features from native api. Dont abuse it:)
    /// </summary>
    public enum MessageDefinitions : uint
    {
        Null = 0x0000,
        Quit = 0x0012,
        GetTextLength = 0x000E,
        GetText = 0x000D
    }
}