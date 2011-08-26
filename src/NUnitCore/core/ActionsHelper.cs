// ****************************************************************
// Copyright 2011, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

#if CLR_2_0 || CLR_4_0
using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core
{
    internal class ActionsHelper
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

        public static object[] GetActionsFromTypeAttributes(Type type)
        {
            if(type == typeof(object))
                return new object[0];

            ArrayList actions = new ArrayList();

            actions.AddRange(GetActionsFromTypeAttributes(type.BaseType));

            Type[] declaredInterfaces = GetDeclaredInterfaces(type);

            foreach(Type interfaceType in declaredInterfaces)
                actions.AddRange(GetActionsFromAttributes(interfaceType));

            actions.AddRange(GetActionsFromAttributes(type));

            return actions.ToArray();
        }

        private static Type[] GetDeclaredInterfaces(Type type)
        {
            Type[] interfaces = type.GetInterfaces();
            Type[] baseInterfaces = new Type[0];

            if (type.BaseType != typeof(object))
                return interfaces;

            ArrayList declaredInterfaces = new ArrayList();
            foreach (Type interfaceType in interfaces)
            {
                if (Array.IndexOf(baseInterfaces, interfaceType) < 0)
                    declaredInterfaces.Add(interfaceType);
            }

            return (Type[])declaredInterfaces.ToArray(typeof(Type));
        }

        public static object[] GetActionsFromAttributes(ICustomAttributeProvider attributeProvider)
        {
            ArrayList resultList = new ArrayList();

            object[] attributes = attributeProvider.GetCustomAttributes(false);

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
                return;

            Type actionType = GetActionType(level);
            MethodInfo actionMethod = GetActionMethod(actionType, level, phase);

            object[] filteredActions = GetFilteredAndSortedActions(actions, phase, actionType);

            foreach (object action in filteredActions)
            {
                if (action == null)
                    continue;

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

    public enum ActionLevel
    {
        Suite,
        Test
    }

    public enum ActionPhase
    {
        Before,
        After
    }
}
#endif
