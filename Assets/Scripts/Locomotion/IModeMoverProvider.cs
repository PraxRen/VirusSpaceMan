public interface IModeMoverProvider
{
    ModeMover DefaultModeMover { get; }
    ModeMover ActiveModeMover { get; }
}