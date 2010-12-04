using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NUnit.Core
{
    internal static class BehaviorsHelper
    {
        private static Type _BehaviorInterfaceType = null;
        private static MethodInfo _BehaviorPriorityGetter = null;
        private static Hashtable _BehaviorTypes = null;

        static BehaviorsHelper()
        {
            _BehaviorInterfaceType = Type.GetType(NUnitFramework.BehaviorInterface);
            _BehaviorPriorityGetter = _BehaviorInterfaceType
                .GetMethod("get_Priority");

            _BehaviorTypes = new Hashtable();

            _BehaviorTypes.Add(BehaviorLevel.Assembly, Type.GetType(NUnitFramework.AssemblyBehaviorInterface));
            _BehaviorTypes.Add(BehaviorLevel.Suite, Type.GetType(NUnitFramework.SuiteBehaviorInterface));
            _BehaviorTypes.Add(BehaviorLevel.Test, Type.GetType(NUnitFramework.TestBehaviorInterface));
            _BehaviorTypes.Add(BehaviorLevel.TestCase, Type.GetType(NUnitFramework.TestCaseBehaviorInterface));
        }

        public static Attribute[] GetBehaviorAttributes(ICustomAttributeProvider attributeProvider)
        {
            ArrayList resultList = new ArrayList();

            object[] attributes = attributeProvider.GetCustomAttributes(true);

            foreach (Attribute attribute in attributes)
            {
                if (_BehaviorInterfaceType.IsAssignableFrom(attribute.GetType()))
                    resultList.Add(attribute);
            }

            Attribute[] results = new Attribute[resultList.Count];
            resultList.CopyTo(results);

            return results;
        }

        public static void ExecuteBehaviors(BehaviorLevel level, BehaviorPhase phase, IEnumerable behaviors, object fixture)
        {
            if (behaviors == null)
                throw new ArgumentNullException("behaviors");

            Type behaviorType = GetBehaviorType(level);
            MethodInfo behaviorAction = GetBehaviorAction(behaviorType, level, phase);

            Attribute[] sortedBehaviors = GetFilteredAndSortedBehaviors(behaviors, phase, behaviorType);

            foreach (object behavior in sortedBehaviors)
            {
                if (behavior == null)
                    continue;

                Reflect.InvokeMethod(behaviorAction, behavior, fixture);
            }
        }

        private static Attribute[] GetFilteredAndSortedBehaviors(IEnumerable behaviors, BehaviorPhase phase, Type behaviorType)
        {
            ArrayList filteredBehaviors = new ArrayList();
            foreach(object behaviorItem in behaviors)
            {
                if(behaviorItem == null)
                    continue;

                if(behaviorItem is IEnumerable)
                {
                    foreach(object nestedItem in ((IEnumerable)behaviorItem))
                    {
                        if(nestedItem == null)
                            continue;

                        if (behaviorType.IsAssignableFrom(nestedItem.GetType()))
                            filteredBehaviors.Add(nestedItem);
                    }
                }
                else if(behaviorType.IsAssignableFrom(behaviorItem.GetType()))
                    filteredBehaviors.Add(behaviorItem);
            }

            Attribute[] sortedBehaviors = new Attribute[filteredBehaviors.Count];
            filteredBehaviors.CopyTo(sortedBehaviors);

            SortBehaviorAttributes(sortedBehaviors, phase == BehaviorPhase.Before ? true : false);
            return sortedBehaviors;
        }

        private static Type GetBehaviorType(BehaviorLevel level)
        {
            return (Type) _BehaviorTypes[level];
        }

        private static MethodInfo GetBehaviorAction(Type behaviorType, BehaviorLevel level, BehaviorPhase phase)
        {
            if (phase == BehaviorPhase.Before)
            {
                if (level == BehaviorLevel.Assembly)
                    return Reflect.GetNamedMethod(behaviorType, "BeforeAllTests");

                if (level == BehaviorLevel.Suite)
                    return Reflect.GetNamedMethod(behaviorType, "BeforeSuite");

                if (level == BehaviorLevel.Test)
                    return Reflect.GetNamedMethod(behaviorType, "BeforeTest");

                return Reflect.GetNamedMethod(behaviorType, "BeforeTestCase");
            }

            if (level == BehaviorLevel.Assembly)
                return Reflect.GetNamedMethod(behaviorType, "AfterAllTests");

            if (level == BehaviorLevel.Suite)
                return Reflect.GetNamedMethod(behaviorType, "AfterSuite");

            if (level == BehaviorLevel.Test)
                return Reflect.GetNamedMethod(behaviorType, "AfterTest");

            return Reflect.GetNamedMethod(behaviorType, "AfterTestCase");
        }

        private static void SortBehaviorAttributes(Attribute[] attributes, bool inReversePriority)
        {
            if (inReversePriority)
                Array.Sort(attributes, CompareBehaviorAttributePriorityInReverseOfNaturalOrder);
            else
                Array.Sort(attributes, CompareBehaviorAttributePriorityInNaturalOrder);
        }

        private static int CompareBehaviorAttributePriorityInNaturalOrder(Attribute left, Attribute right)
        {
            return ((int)_BehaviorPriorityGetter.Invoke(left, null)).CompareTo((int)_BehaviorPriorityGetter.Invoke(right, null));
        }

        private static int CompareBehaviorAttributePriorityInReverseOfNaturalOrder(Attribute left, Attribute right)
        {
            return ((int)_BehaviorPriorityGetter.Invoke(right, null)).CompareTo((int)_BehaviorPriorityGetter.Invoke(left, null));
        }
    }

    internal enum BehaviorLevel
    {
        Assembly,
        Suite,
        Test,
        TestCase
    }

    internal enum BehaviorPhase
    {
        Before,
        After
    }
}
