public class StatePursuit : StateMoverToTarget, IModeMoverProvider
{
    public StatePursuit(string id, AICharacter character, float timeSecondsWaitHandle, ModeMover modeMover) : base(id, character, timeSecondsWaitHandle) 
    {
        ModeMover = modeMover;
    }

    public ModeMover ModeMover { get; }
}