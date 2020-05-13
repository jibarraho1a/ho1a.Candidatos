using System;
using System.Collections.Generic;
using System.Text;

namespace ho1a.applicationCore.Data.Interfaces
{
    public interface IAppLogger<T>
    {
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
    }
}
