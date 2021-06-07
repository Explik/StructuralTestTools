using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    public abstract class EventDescription : MemberDescription
    {
        public abstract MethodDescription AddMethod { get; }

        // TODO add Attributes

        public abstract TypeDescription EventHandlerType { get; }

        public override MemberTypes MemberType => MemberTypes.Event;

        public abstract MethodDescription RemoveMethod { get; }
    }
}
