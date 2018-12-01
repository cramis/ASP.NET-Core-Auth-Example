using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Dau.ORM
{
    interface IRepositoryString
    {
        string SelectString<T>(T model);

        string SelectString<T>(T model, params ColumnInfo[] args);
        string InsertStr<T>(T model);
        string UpdateStr<T>(T model);
        string MergeStr<T>(T model);
        string DeleteStr<T>(T model);


    }
    public class BaseRepositoryString : IRepositoryString
    {

        private readonly IORMHelper helper;

        private readonly string ParamMark = ":";
        private readonly string DBNowDatefunction = "sysdate";


        public BaseRepositoryString(IORMHelper helper)
        {
            this.helper = helper;
        }

        public BaseRepositoryString(IORMHelper helper, string ParamMark, string DBNowDatefunction)
        {
            this.helper = helper;
            this.ParamMark = ParamMark;
            this.DBNowDatefunction = DBNowDatefunction;

        }

        public string SelectString<T>(T model)
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

            return str.ToString().TrimEnd();
        }

        public string SelectString<T>(T model, params ColumnInfo[] args)
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
                        operatorAndvalues.AppendFormat("{0} {1} ", a.Operator.ToUpper(), a.Operator_values[0]);
                        break;
                }

                str.AppendFormat("AND {0}.{1} {2}", tableName, a.COLUMN_NAME, operatorAndvalues);
                count++;

            }

            return str.ToString().TrimEnd();
        }
        public string InsertStr<T>(T model)
        {
            StringBuilder str = new StringBuilder("");
            string tableName = helper.GetTableName(model.GetType());
            var props = model.GetType().GetProperties();

            long count = 0;
            long pk_Count = 0;
            str.AppendFormat("INSERT INTO {0} ( ", tableName);

            foreach (var p in props)
            {
                if (!helper.CheckIgnore(p))
                {
                    var columnName = helper.ColumnName(p);

                    if (p.GetValue(model) != null || helper.CheckCreatedDate(p))
                    {
                        if (helper.CheckKey(p))
                        {
                            pk_Count++;
                        }
                        if (count >= 1)
                        {
                            str.Append(", ");
                        }
                        str.Append(columnName);
                        count++;
                    }
                }
            }

            if (pk_Count < 1)
            {
                throw new Exception("Insert를 할때에는 Primary Key가 한개 이상 존재해야 합니다.");
            }

            str.Append(" ) VALUES ( ");

            count = 0;
            foreach (var p in props)
            {
                if (!helper.CheckIgnore(p))
                {
                    var columnName = helper.ColumnName(p);

                    if (p.GetValue(model) != null || helper.CheckCreatedDate(p))
                    {
                        if (count >= 1)
                        {
                            str.Append(", ");
                        }

                        if (helper.CheckCreatedDate(p))
                        {
                            str.Append("sysdate");
                        }
                        else
                        {
                            str.Append(ParamMark + p.Name);
                        }
                        count++;
                    }
                }
            }
            str.Append(" )");

            return str.ToString().TrimEnd();
        }
        public string UpdateStr<T>(T model)
        {
            StringBuilder str = new StringBuilder("");
            string tableName = helper.GetTableName(model.GetType());
            var props = model.GetType().GetProperties();

            long count = 0;

            str.AppendFormat("UPDATE {0} SET ", tableName);
            foreach (var p in props)
            {
                var columnName = helper.ColumnName(p);


                if (p.GetValue(model) != null || helper.CheckLastModifiedDate(p))
                {
                    if (!helper.CheckIgnore(p))
                    {
                        if (count >= 1)
                        {
                            str.Append(", ");
                        }

                        if (helper.CheckLastModifiedDate(p))
                        {
                            str.AppendFormat("{0} = {1}", columnName, DBNowDatefunction);
                        }
                        else
                        {
                            str.AppendFormat("{0} = {1}{2}", columnName, ParamMark, p.Name);
                        }
                        count++;
                    }
                }
            }
            str.Append(" WHERE ");

            count = 0;
            foreach (var p in props)
            {
                var columnName = helper.ColumnName(p);

                if (helper.CheckKey(p))
                {
                    if (p.GetValue(model) == null)
                    {
                        throw new Exception("Update를 할때에는 Primary Key에 반드시 값이 존재해야 합니다.");
                    }

                    if (count >= 1)
                    {
                        str.Append(" AND ");
                    }
                    str.AppendFormat("{0} = {1}{2}", columnName, ParamMark, p.Name);
                    count++;
                }

            }


            return str.ToString().TrimEnd();
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

    public class OracleRepositoryString : BaseRepositoryString
    {
        public OracleRepositoryString(IORMHelper helper) : base(helper, ":", "sysdate")
        {
        }
    }
}