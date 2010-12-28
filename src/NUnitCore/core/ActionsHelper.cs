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
        private static Hashtable _ActionTypes = null;

        static ActionsHelper()
        {
            _ActionInterfaceType = Type.GetType(NUnitFramework.TestActionInterface);
            _ActionTypes = new Hashtable();

            _ActionTypes.Add(ActionLevel.Suite, Type.GetType(NUnitFramework.TestSuiteActionInterface));
            _ActionTypes.Add(ActionLevel.Test, Type.GetType(NUnitFramework.TestCaseActionInterface));
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

        public static void ExecuteActions(ActionLevel level, ActionPhase phase, IEnumerable actions, object fixture, MethodInfo method)
        {
            if (actions == null)
                throw new ArgumentNullException("actions");

            Type actionType = GetActionType(level);
            MethodInfo actionMethod = GetActionMethod(actionType, level, phase);

            object[] filteredActions = GetFilteredAndSortedActions(actions, phase, actionType);

            

            foreach (object action in filteredActions)
            {
                if (action == null)
                    continue;
                
                if(level == ActionLevel.Suite)
                    Reflect.InvokeMethod(actionMethod, action, fixture);
                else
                    Reflect.InvokeMethod(actionMethod, action, fixture, method);
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

            if(phase == ActionPhase.After)
                filteredActions.Reverse();

            return filteredActions.ToArray();
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
                    return Reflect.GetNamedMethod(actionType, "BeforeTestSuite");

                return Reflect.GetNamedMethod(actionType, "BeforeTestCase");
            }

            if (level == ActionLevel.Suite)
                return Reflect.GetNamedMethod(actionType, "AfterTestSuite");

            return Reflect.GetNamedMethod(actionType, "AfterTestCase");
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
