using UnityEngine;

namespace EventAggregation
{
    public class SimpleEvent : IEventBase { }

    public class IntEvent : IEventBase { public int Value { get; set; } } 

    public class MouseLeftClickEvent : IEventBase { public Ray Point { get; set; } }
}