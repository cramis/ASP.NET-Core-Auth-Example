using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Dau.ORM
{
    interface ICRUDString
    {
        string GetListString<T>(T model);

        string GetListString<T>(T model, params ColumnInfo[] args);
        string GetItemString<T>(T model);
        string InsertStr<T>(T model);
        string UpdateStr<T>(T model);
        string MergeStr<T>(T model);
        string DeleteStr<T>(T model);


    }
    public class OracleCRUDString : ICRUDString
    {
        private readonly IORMHelper helper;

        public OracleCRUDString(IORMHelper helper)
        {
            this.helper = helper;
        }

        public string GetListString<T>(T model)
        {
            StringBuilder str = new StringBuilder("");
            string tableName = helper.GetTableName(model.GetType());
            str.AppendFormat("SELECT * FROM {0}", tableName);
            str.Append(" WHERE 1=1 ");

            var props = model.GetType().GetProperties();

            int count = 0;
            foreach (var p in props)
            {
                var columnName = helper.ColumnName(p);

                if (p.GetValue(model) != null)
                {
                    str.AppendFormat("AND {0}.{1} = :{2}", tableName, columnName, p.Name);
                    count++;
                }

            }

            return str.ToString();
        }

        public string GetListString<T>(T model, params ColumnInfo[] args)
        {
            StringBuilder str = new StringBuilder("");
            string tableName = helper.GetTableName(model.GetType());
            str.AppendFormat("SELECT * FROM {0}", tableName);
            str.Append(" WHERE 1=1 ");

            var props = model.GetType().GetProperties();

            int count = 0;
            foreach (var a in args)
            {
                if (string.IsNullOrWhiteSpace(a.Operator))
                {
                    throw new Exception("연산자를 존재하지 않습니다. 연산자가 반드시 입력되어야 합니다. 예) '>', '<=', 'between' 등 ");
                }

                StringBuilder operatorAndvalues = new StringBuilder();

                switch (a.Operator.ToUpper())
                {
                    case "BETWEEN":
                        operatorAndvalues.AppendFormat("BETWEEN {0} AND {1} ", a.Operator_values[0], a.Operator_values[1]);
                        break;
                    case "IS NULL":
                        operatorAndvalues.Append("IS NULL ");
                        break;
                    case "IS NOT NULL":
                        operatorAndvalues.Append("IS NOT NULL ");
                        break;
                    default:
                        operatorAndvalues.AppendFormat("{0} {1} ", a.Operator, a.Operator_values[0]);
                        break;
                }

                str.AppendFormat("AND {0}.{1} {2}", tableName, a.COLUMN_NAME, operatorAndvalues);
                count++;

            }

            return str.ToString();
        }
        public string GetItemString<T>(T model)
        {
            throw new NotImplementedException();
        }
        public string InsertStr<T>(T model)
        {
            throw new NotImplementedException();
        }
        public string UpdateStr<T>(T model)
        {
            throw new NotImplementedException();
        }
        public string MergeStr<T>(T model)
        {
            throw new NotImplementedException();
        }
        public string DeleteStr<T>(T model)
        {
            throw new NotImplementedException();
        }
    }
}