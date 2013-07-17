using System;
using System.Linq.Expressions;
using System.Management.Automation;

namespace PSClrMD.Tests
{
    public static class PowerShellExtensions
    {
        public static PowerShell AddCommand<TCmdlet>(this PowerShell shell) where TCmdlet : PSCmdlet
        {
            return AddCommand<TCmdlet>(shell, null);
        }

        public static PowerShell AddCommand<TCmdlet>(this PowerShell shell, Action<IPSCmdlet<TCmdlet>> commandAction) where TCmdlet : PSCmdlet
        {
            var commandType = typeof(TCmdlet);
            var cmdletAttribute = (CmdletAttribute)commandType.GetCustomAttributes(typeof(CmdletAttribute), false)[0];
            shell.AddCommand(string.Format("{0}-{1}", cmdletAttribute.VerbName, cmdletAttribute.NounName));
            if (commandAction != null)
            {
                var adapter = new PSCmdletAdapter<TCmdlet>(shell);
                commandAction(adapter);
            }
            return shell;
        }

        class PSCmdletAdapter<TCmdlet> : IPSCmdlet<TCmdlet> where TCmdlet : PSCmdlet
        {
            private readonly PowerShell _shell;

            public PSCmdletAdapter(PowerShell shell)
            {
                _shell = shell;
            }

            public IPSCmdlet<TCmdlet> AddParameter<TParameter>(Expression<Func<TCmdlet, TParameter>> parameterLambda, object value)
            {
                var memberExpression = parameterLambda.Body as MemberExpression;
                if (memberExpression == null) throw new ArgumentException("Should be a member expression.", "parameterLambda");
                var parameterName = memberExpression.Member.Name;

                _shell.AddParameter(parameterName, value);
                return this;
            }
        }
    }

    public interface IPSCmdlet<TCmdlet> where TCmdlet : PSCmdlet
    {
        IPSCmdlet<TCmdlet> AddParameter<TParameter>(Expression<Func<TCmdlet, TParameter>> parameterLambda, object value);
    }
}