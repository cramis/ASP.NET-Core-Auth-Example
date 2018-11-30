using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Dapper;

namespace Dau.ORM
{
    public interface IORMHelper
    {
        string GetTableName(Type classType);
        bool CheckKey(PropertyInfo propInfo);
        string ColumnName(PropertyInfo p);
        bool CheckIgnore(PropertyInfo propInfo);
        bool CheckCreatedDate(PropertyInfo propInfo);
        bool CheckLastModifiedDate(PropertyInfo propInfo);
    }
    public class BaseORMHelper : IORMHelper
    {

        public virtual string GetTableName(Type classType)
        {
            string tblName = "";

            var attrs = classType.GetCustomAttributes(true);

            foreach (var attr in attrs)
            {
                if (attr is Table)
                {
                    Table tbl = (Table)attr;
                    tblName = tbl.GetName();
                }

            }
            return tblName;
        }




        public virtual bool CheckKey(PropertyInfo propInfo)
        {
            bool isOK = false;

            var attrs = propInfo.GetCustomAttributes(true);
            var name = propInfo.Name;

            foreach (var attr in attrs)
            {
                if (attr is KeyColumn)
                {
                    isOK = true;
                }

            }
            return isOK;
        }

        public virtual string ColumnName(PropertyInfo p)
        {
            var columnName = p.Name;

            var attrs = p.GetCustomAttributes(true);

            foreach (var attr in attrs)
            {
                if (attr is BindingColumn)
                {
                    BindingColumn a = (BindingColumn)attr;
                    columnName = a.getName();
                }

            }

            return columnName;
        }

        public virtual string ColumnName(object table, string column)
        {
            var p = table.GetType().GetProperty(column);

            var columnName = p.Name;

            var attrs = p.GetCustomAttributes(true);

            foreach (var attr in attrs)
            {
                if (attr is BindingColumn)
                {
                    BindingColumn a = (BindingColumn)attr;
                    columnName = a.getName();
                }

            }

            return columnName;
        }


        public virtual bool CheckIgnore(PropertyInfo propInfo)
        {
            bool isOK = false;

            var attrs = propInfo.GetCustomAttributes(true);
            var name = propInfo.Name;

            foreach (var attr in attrs)
            {
                if (attr is IgnoreColumn)
                {
                    isOK = true;
                }

            }
            return isOK;
        }


        public virtual bool CheckCreatedDate(PropertyInfo propInfo)
        {
            bool isOK = false;

            var attrs = propInfo.GetCustomAttributes(true);
            var name = propInfo.Name;

            foreach (var attr in attrs)
            {
                if (attr is CreatedDate)
                {
                    isOK = true;
                }

            }
            return isOK;
        }

        public virtual bool CheckLastModifiedDate(PropertyInfo propInfo)
        {
            bool isOK = false;

            var attrs = propInfo.GetCustomAttributes(true);
            var name = propInfo.Name;

            foreach (var attr in attrs)
            {
                if (attr is LastModifiedDate)
                {
                    isOK = true;
                }
            }
            return isOK;
        }


    }

    public class OracleORMHelper : BaseORMHelper
    {

    }

    public class ColumnInfo
    {
        public ColumnInfo()
        {

        }

        public ColumnInfo(string COLUMN_NAME, string Operator, params string[] Operator_values)
        {
            this.COLUMN_NAME = COLUMN_NAME;
            this.Operator = Operator;
            this.Operator_values = Operator_values;
        }


        public string Operator { get; set; }
        public string COLUMN_NAME { get; set; }

        public string COLUMN_VALUE { get; set; }

        public string[] Operator_values { get; set; }
    }
}