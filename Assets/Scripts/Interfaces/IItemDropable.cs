using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemDropable
{
    List<Item> Items { get; set; }

    void DropItem();
}
