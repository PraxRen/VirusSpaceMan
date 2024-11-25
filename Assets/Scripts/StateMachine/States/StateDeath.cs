public class StateDeath : State
{
    public StateDeath(string id, Character character, float timeSecondsWaitHandle) : base(id, character, timeSecondsWaitHandle) { }

    protected override void EnterAfterAddon()
    {
        Character.enabled = false;
    }
}