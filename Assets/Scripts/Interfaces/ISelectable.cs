using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    bool Selected { get; set; }
    Dictionary<string, object> Info { get; set; }
    Dictionary<string, object> GetInfo();
}
