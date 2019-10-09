﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructable
{
    int Health { get; set; }
    int MaxHealth { get; set; }

    void TakeDamage(int count);
}