
using System.Collections;
using System.Reflection;

namespace NUnit.Core.Extensibility
{
    class ParameterProviders : ExtensionPoint, IParameterProvider
    {
        public ParameterProviders(IExtensionHost host) : base( "ParameterProviders", host ) { }

        #region IParameterProvider Members

        /// <summary>
        /// Determine whether any ParameterSets
        /// are available for a method.
        /// </summary>
        /// <param name="method">A MethodInfo representing the a parameterized test</param>
        /// <returns>True if any are available, otherwise false.</returns>
        public bool HasParametersFor(MethodInfo method)
        {
            foreach (IParameterProvider provider in Extensions)
                if (provider.HasParametersFor(method))
                    return true;

            return false;
        }

        /// <summary>
        /// Return a list providing ParameterSets
        /// for use in running a test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IList GetParametersFor(MethodInfo method)
        {
            ArrayList paramList = new ArrayList();

            foreach( IParameterProvider provider in Extensions )
                if ( provider.HasParametersFor(method ) )
                    paramList.AddRange( provider.GetParametersFor(method) );

            return paramList;
        }
        #endregion

        #region IsValidExtension
        protected override bool IsValidExtension(object extension)
        {
            return extension is IParameterProvider;
        }
        #endregion
    }
}
