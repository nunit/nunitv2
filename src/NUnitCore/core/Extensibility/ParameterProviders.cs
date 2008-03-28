
using System.Collections;
using System.Reflection;

namespace NUnit.Core.Extensibility
{
    class ParameterProviders : ExtensionPoint, IParameterProvider
    {
        public ParameterProviders(IExtensionHost host) : base( "ParameterProviders", host ) { }

        #region IParameterProvider Members
        public bool HasParametersFor(MethodInfo method)
        {
            foreach (IParameterProvider provider in Extensions)
                if (provider.HasParametersFor(method))
                    return true;

            return false;
        }

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
