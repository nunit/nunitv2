using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NUnit.Core
{
    internal static class ActionsHelper
    {
        private static Type _ActionInterfaceType = null;
        private static MethodInfo _ActionPriorityGetter = null;
        private static Hashtable _ActionTypes = null;

        static ActionsHelper()
        {
            _ActionInterfaceType = Type.GetType(NUnitFramework.ActionInterface);
            _ActionPriorityGetter = _ActionInterfaceType
                .GetMethod("get_Priority");

            _ActionTypes = new Hashtable();

            _ActionTypes.Add(ActionLevel.Suite, Type.GetType(NUnitFramework.SuiteActionInterface));
            _ActionTypes.Add(ActionLevel.Test, Type.GetType(NUnitFramework.TestActionInterface));
        }

        public static object[] GetActionsFromAttributes(ICustomAttributeProvider attributeProvider)
        {
            ArrayList resultList = new ArrayList();

            object[] attributes = attributeProvider.GetCustomAttributes(true);

            foreach (Attribute attribute in attributes)
            {
                if (_ActionInterfaceType.IsAssignableFrom(attribute.GetType()))
                    resultList.Add(attribute);
            }

            object[] results = new object[resultList.Count];
            resultList.CopyTo(results);

            return results;
        }

        public static void ExecuteActions(ActionLevel level, ActionPhase phase, IEnumerable actions, object fixture)
        {
            if (actions == null)
                throw new ArgumentNullException("actions");

            Type actionType = GetActionType(level);
            MethodInfo actionMethod = GetActionMethod(actionType, level, phase);

            object[] sortedActions = GetFilteredAndSortedActions(actions, phase, actionType);

            foreach (object action in sortedActions)
            {
                if (action == null)
                    continue;

                Reflect.InvokeMethod(actionMethod, action, fixture);
            }
        }

        private static object[] GetFilteredAndSortedActions(IEnumerable actions, ActionPhase phase, Type actionType)
        {
            ArrayList filteredActions = new ArrayList();
            foreach(object actionItem in actions)
            {
                if(actionItem == null)
                    continue;

                if(actionItem is IEnumerable)
                {
                    foreach(object nestedItem in ((IEnumerable)actionItem))
                    {
                        if(nestedItem == null)
                            continue;

                        if (actionType.IsAssignableFrom(nestedItem.GetType()) && filteredActions.Contains(nestedItem) != true)
                            filteredActions.Add(nestedItem);
                    }
                }
                else if(actionType.IsAssignableFrom(actionItem.GetType()) && filteredActions.Contains(actionItem) != true)
                    filteredActions.Add(actionItem);
            }

            object[] sortedActions = new object[filteredActions.Count];
            filteredActions.CopyTo(sortedActions);

            SortActions(sortedActions, phase == ActionPhase.Before ? true : false);
            return sortedActions;
        }

        private static Type GetActionType(ActionLevel level)
        {
            return (Type) _ActionTypes[level];
        }

        private static MethodInfo GetActionMethod(Type actionType, ActionLevel level, ActionPhase phase)
        {
            if (phase == ActionPhase.Before)
            {
                if (level == ActionLevel.Suite)
                    return Reflect.GetNamedMethod(actionType, "BeforeSuite");

                return Reflect.GetNamedMethod(actionType, "BeforeTest");
            }

            if (level == ActionLevel.Suite)
                return Reflect.GetNamedMethod(actionType, "AfterSuite");

            return Reflect.GetNamedMethod(actionType, "AfterTest");
        }

        private static void SortActions(object[] actions, bool inReversePriority)
        {
            if (inReversePriority)
                Array.Sort(actions, CompareActionPriorityInReverseOfNaturalOrder);
            else
                Array.Sort(actions, CompareActionPriorityInNaturalOrder);
        }

        private static int CompareActionPriorityInNaturalOrder(object left, object right)
        {
            return ((int)_ActionPriorityGetter.Invoke(left, null)).CompareTo((int)_ActionPriorityGetter.Invoke(right, null));
        }

        private static int CompareActionPriorityInReverseOfNaturalOrder(object left, object right)
        {
            return ((int)_ActionPriorityGetter.Invoke(right, null)).CompareTo((int)_ActionPriorityGetter.Invoke(left, null));
        }
    }

    internal enum ActionLevel
    {
        Suite,
        Test
    }

    internal enum ActionPhase
    {
        Before,
        After
    }
}
