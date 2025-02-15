using System.Collections.Generic;

public interface ISerializationStrategy
{
    void SaveFile(Dictionary<string, object> state, string fileName);

    Dictionary<string, object> LoadFile(string fileName);

    void DeleteFile(string fileName);
}