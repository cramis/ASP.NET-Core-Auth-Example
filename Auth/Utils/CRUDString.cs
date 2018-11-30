using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Dau.Util
{
    public interface ICRUDString
    {
        string GetListString<T>(T model);
        string GetItemString<T>(T model);
        string InsertStr<T>(T model);
        string UpdateStr<T>(T model);
        string MergeStr<T>(T model);
        string DeleteStr<T>(T model);


    }
    public class OracleCRUDString : ICRUDString
    {
        public string DeleteStr<T>(T model)
        {
            throw new NotImplementedException();
        }

        public string GetItemString<T>(T model)
        {
            throw new NotImplementedException();
        }

        public string GetListString<T>(T model)
        {
            throw new NotImplementedException();
        }

        public string InsertStr<T>(T model)
        {
            throw new NotImplementedException();
        }

        public string MergeStr<T>(T model)
        {
            throw new NotImplementedException();
        }

        public string UpdateStr<T>(T model)
        {
            throw new NotImplementedException();
        }
    }
}