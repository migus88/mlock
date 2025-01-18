using System;

namespace Sandland.LockSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LockableBehaviourAttribute : Attribute
    {
        public LockableBehaviourAttribute(string serviceAccess)
        {
            
        }
    }
}