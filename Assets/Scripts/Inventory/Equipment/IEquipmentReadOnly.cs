using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipmentReadOnly
{
    public event Action<IEquipmentCellReanOnly> ChangedCell;
}
