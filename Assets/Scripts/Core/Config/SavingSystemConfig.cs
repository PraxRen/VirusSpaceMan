using System;
using UnityEngine;

[Serializable]
public class SavingSystemConfig
{
    [SerializeField] private string _fileName;
    [SerializeField] private TypeSerializationStrategy _typeSerializationStrategy;

    private ISerializationStrategy _strategy;

    public SavingSystemConfig(string fileName, TypeSerializationStrategy typeSerializationStrategy)
    {
        _fileName = fileName;
        _typeSerializationStrategy = typeSerializationStrategy;
        _strategy = GetStrategy();
    }

    public string FileName => _fileName;
    public TypeSerializationStrategy TypeSerializationStrategy => _typeSerializationStrategy;
    public ISerializationStrategy Strategy
    { 
        get 
        {
            if ( _strategy == null )
                _strategy = GetStrategy();

            return _strategy;
        } 
    }


    private ISerializationStrategy GetStrategy()
    {
        return _typeSerializationStrategy switch
        {
            TypeSerializationStrategy.Binary => new BinarySerializationStrategy(),
            _ => throw new InvalidOperationException("")
        };
    }
}