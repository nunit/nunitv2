using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NUnit.Core
{
    internal static class BehaviorsHelper
    {
        public static void ExecuteBehaviors(IEnumerable behaviorAttributeSets, object fixture, string method, bool inReversePriority)
        {
            if(behaviorAttributeSets == null)
                throw new ArgumentNullException("behaviorAttributeSets");

            MethodInfo behaviorMethod = null;

            foreach (Attribute[] behaviorAttributes in behaviorAttributeSets)
            {
                if(behaviorAttributes == null)
                    continue;

                Attribute[] sortedBehaviors = (Attribute[])behaviorAttributes.Clone();
                SortBehaviorAttributes(sortedBehaviors, inReversePriority);

                foreach (Attribute behaviorAttribute in sortedBehaviors)
                {
                    if (behaviorMethod == null)
                    {
                        Type behaviorAttributeType = behaviorAttribute.GetType();
                        while (behaviorAttributeType.FullName != NUnitFramework.BehaviorAttribute)
                            behaviorAttributeType = behaviorAttributeType.BaseType;

                        behaviorMethod = Reflect.GetNamedMethod(behaviorAttributeType, method);
                    }

                    Reflect.InvokeMethod(behaviorMethod, behaviorAttribute, fixture);
                }
            }
        }

        private static void SortBehaviorAttributes(Attribute[] attributes, bool inReversePriority)
        {
            if (inReversePriority)
                Array.Sort(attributes, CompareBehaviorAttributePriorityInNaturalOrder);
            else
                Array.Sort(attributes, CompareBehaviorAttributePriorityInReverseOfNaturalOrder);
        }

        private static int CompareBehaviorAttributePriorityInNaturalOrder(Attribute left, Attribute right)
        {
            return ((int)Reflect.GetPropertyValue(left, "Priority")).CompareTo((int)Reflect.GetPropertyValue(right, "Priority"));
        }

        private static int CompareBehaviorAttributePriorityInReverseOfNaturalOrder(Attribute left, Attribute right)
        {
            return ((int)Reflect.GetPropertyValue(right, "Priority")).CompareTo((int)Reflect.GetPropertyValue(left, "Priority"));
        }
    }
}
