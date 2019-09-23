using UnityEngine;

namespace EventAggregation
{
    public class SimpleEvent : IEventBase
    {

    }

    public class IntEvent : IEventBase
    {
        public int Value { get; set; }
    }

    public class SelectEvent : IEventBase
    {
        public RaycastHit hit { get; set; } 
    }

    public class MultiSelectEvent : IEventBase
    {
        public Vector3 firstCorner { get; set; }
        public Vector3 secondCorner { get; set; }
    }

    public class DeselectEvent : IEventBase
    {

    }
}