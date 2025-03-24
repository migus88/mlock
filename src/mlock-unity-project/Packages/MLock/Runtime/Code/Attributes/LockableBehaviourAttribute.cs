using System;

namespace Migs.MLock.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LockableBehaviourAttribute : Attribute
    {
        public LockableBehaviourAttribute(string serviceAccess)
        {
            
        }
    }
}