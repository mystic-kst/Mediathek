using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DataAccessStore
{
    class DataAccessException : Exception
    {
        public object Origin { get; set; }

        public DataAccessException(string message)
            : base(message)
        {

        }

        public DataAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
            TraceException(innerException);
        }


        public DataAccessException(object origin, string message, Exception innerException)
            : base(message, innerException)
        {
            Origin = origin;
            TraceException(innerException);
        }

        private void TraceException(Exception exception)
        {
            Trace.WriteLine("---------------------------------------------------------");
            Trace.WriteLine(DateTime.Now.ToString());
            //Trace.WriteLineIf(Origin != null, "Error origin: " + Origin.ToString());
            Trace.TraceError(exception.Message);
            Trace.WriteLine(exception.StackTrace);
        }
    }
}
