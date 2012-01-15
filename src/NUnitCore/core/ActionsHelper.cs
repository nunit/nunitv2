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
        private static Type _TestDetailsClassType = null;

        static ActionsHelper()
        {
            _ActionInterfaceType = Type.GetType(NUnitFramework.TestActionInterface);
            _TestDetailsClassType = Type.GetType(NUnitFramework.TestDetailsClass);
        }

        public static void ExecuteActions(ActionPhase phase, IEnumerable actions, object testDetails)
        {
            if (actions == null)
                return;

            object[] filteredActions = GetFilteredAndSortedActions(actions, phase);

            MethodInfo actionMethod = GetActionMethod(phase);
            foreach (object action in filteredActions)
            {
                if (action == null)
                    continue;

                Reflect.InvokeMethod(actionMethod, action, testDetails);
            }
        }

        public static object CreateTestDetails(ITest test, object fixture, MethodInfo method)
        {
            return Activator.CreateInstance(_TestDetailsClassType,
                                            fixture,
                                            method,
                                            test.TestName.FullName,
                                            test.TestType,
                                            test.IsSuite);
        }

        public static object[] GetActionsFromAttributeProvider(ICustomAttributeProvider attributeProvider)
        {
            if (attributeProvider == null || _ActionInterfaceType == null)
                return new object[0];

            return attributeProvider.GetCustomAttributes(_ActionInterfaceType, false);
        }

        public static object[] GetActionsFromTypesAttributes(Type type)
        {
            if(type == null)
                return new object[0];

            if(type == typeof(object))
                return new object[0];

            ArrayList actions = new ArrayList();

            actions.AddRange(GetActionsFromTypesAttributes(type.BaseType));

            Type[] declaredInterfaces = GetDeclaredInterfaces(type);

            foreach(Type interfaceType in declaredInterfaces)
                actions.AddRange(GetActionsFromAttributeProvider(interfaceType));

            actions.AddRange(GetActionsFromAttributeProvider(type));

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

        private static object[] GetFilteredAndSortedActions(IEnumerable actions, ActionPhase phase)
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

                        if (_ActionInterfaceType.IsAssignableFrom(nestedItem.GetType()) && filteredActions.Contains(nestedItem) != true)
                            filteredActions.Add(nestedItem);
                    }
                }
                else if (_ActionInterfaceType.IsAssignableFrom(actionItem.GetType()) && filteredActions.Contains(actionItem) != true)
                    filteredActions.Add(actionItem);
            }

            if(phase == ActionPhase.After)
                filteredActions.Reverse();

            return filteredActions.ToArray();
        }

        private static MethodInfo GetActionMethod(ActionPhase phase)
        {
            return Reflect.GetNamedMethod(_ActionInterfaceType, phase == ActionPhase.Before ? "BeforeTest" : "AfterTest");
        }

    }

    public enum ActionPhase
    {
        Before,
        After
    }
}
#endif
