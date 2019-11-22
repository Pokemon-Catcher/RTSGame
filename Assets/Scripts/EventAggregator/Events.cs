using UnityEngine;
using System.Collections.Generic;

namespace EventAggregation
{
    public class SimpleEvent : IEventBase
    {

    }

    public class IntEvent : IEventBase
    {
        public int Value { get; set; }
    }

    public class WorldSelectEvent : IEventBase
    {
        public Vector3 firstCorner { get; set; }
        public Vector3 secondCorner { get; set; }
    }

    public class DeselectEvent : IEventBase
    {

    }

    public class ClickUnitSearch : IEventBase
    {
        public Unit Unit { get; set; }
    }
}