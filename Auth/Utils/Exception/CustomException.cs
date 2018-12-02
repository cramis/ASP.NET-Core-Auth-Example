using System;


namespace DapperRepositoryException
{
    public class PkNotFoundException : Exception
    {
        public PkNotFoundException()
        {
        }

        public PkNotFoundException(string message)
            : base(message)
        {
        }

        public PkNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class DBOperatorNotFoundException : Exception
    {
        public DBOperatorNotFoundException()
        {
        }

        public DBOperatorNotFoundException(string message)
            : base(message)
        {
        }

        public DBOperatorNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}



