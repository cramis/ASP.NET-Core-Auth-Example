using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using Xunit;

using Dau.ORM;

namespace Dau.ORM
{
    public class OracleDapperORMTest
    {
        [Fact]
        public void Table_어튜리뷰트_테이블_이름_가져오기()
        {

            OracleORMHelper helper = new OracleORMHelper();
            TestClass t = new TestClass();
            string expect = "TableNm";
            var actual = helper.GetTableName(t.GetType());

            Assert.Equal(expect, actual);
        }

        [Fact]
        public void KeyColumn_어튜리뷰트_PK_컬럼_체크()
        {

            OracleORMHelper helper = new OracleORMHelper();
            TestClass t = new TestClass();
            bool expect = true;
            var actual = helper.CheckKey(t.GetType().GetProperty("Id"));

            Assert.Equal(expect, actual);

            bool expect2 = false;
            var actual2 = helper.CheckKey(t.GetType().GetProperty("Data"));

            Assert.Equal(expect2, actual2);
        }

        [Fact]
        public void IgnoreColumn_어튜리뷰트_무시할컬럼_체크()
        {

            OracleORMHelper helper = new OracleORMHelper();
            TestClass t = new TestClass();
            bool expect = true;
            var actual = helper.CheckIgnore(t.GetType().GetProperty("IgnoreData"));

            Assert.Equal(expect, actual);

            bool expect2 = false;
            var actual2 = helper.CheckIgnore(t.GetType().GetProperty("FakeNameColumn"));

            Assert.Equal(expect2, actual2);

            bool expect3 = false;
            var actual3 = helper.CheckIgnore(t.GetType().GetProperty("Data"));

            Assert.Equal(expect3, actual3);
        }

        [Fact]
        public void BindingColumn_어튜리뷰트_실제바인딩할_컬럼_이름_가져오기()
        {

            OracleORMHelper helper = new OracleORMHelper();
            TestClass t = new TestClass();
            string expect = "Data";
            var actual = helper.ColumnName(t.GetType().GetProperty("Data"));

            Assert.Equal(expect, actual);

            string expect2 = "RealColumnName";
            var actual2 = helper.ColumnName(t.GetType().GetProperty("FakeNameColumn"));

            Assert.Equal(expect2, actual2);

            string expect3 = "CDate";
            var actual3 = helper.ColumnName(t.GetType().GetProperty("CDate"));

            Assert.Equal(expect3, actual3);
        }

        [Fact]
        public void CreatedDate_어튜리뷰트_생성시간_컬럼_체크()
        {

            OracleORMHelper helper = new OracleORMHelper();
            TestClass t = new TestClass();
            bool expect = true;
            var actual = helper.CheckCreatedDate(t.GetType().GetProperty("CDate"));

            Assert.Equal(expect, actual);

            bool expect2 = false;
            var actual2 = helper.CheckCreatedDate(t.GetType().GetProperty("LDate"));

            Assert.Equal(expect2, actual2);

            bool expect3 = false;
            var actual3 = helper.CheckCreatedDate(t.GetType().GetProperty("Data"));

            Assert.Equal(expect3, actual3);
        }

        [Fact]
        public void LastModifiedDate_어튜리뷰트_최종수정시간_컬럼_체크()
        {

            OracleORMHelper helper = new OracleORMHelper();
            TestClass t = new TestClass();
            bool expect = true;
            var actual = helper.CheckLastModifiedDate(t.GetType().GetProperty("LDate"));

            Assert.Equal(expect, actual);

            bool expect2 = false;
            var actual2 = helper.CheckLastModifiedDate(t.GetType().GetProperty("CDate"));

            Assert.Equal(expect2, actual2);

            bool expect3 = false;
            var actual3 = helper.CheckLastModifiedDate(t.GetType().GetProperty("Data"));

            Assert.Equal(expect3, actual3);
        }

        [Fact]
        public void GetListString_쿼리_가져오기()
        {
            OracleORMHelper helper = new OracleORMHelper();

            OracleCRUDString crudString = new OracleCRUDString(helper);
            TestClass t = new TestClass();
            string expect = "SELECT * FROM TableNm WHERE 1=1 ";
            var actual = crudString.GetListString(t);

            Assert.Equal(expect, actual);


            t.Id = 1;
            string expect2 = "SELECT * FROM TableNm WHERE 1=1 AND TableNm.Id = :Id";
            var actual2 = crudString.GetListString(t);

            Assert.Equal(expect2, actual2);
        }

        [Fact]
        public void GetListString_연산자_이용해서_쿼리_가져오기()
        {
            OracleORMHelper helper = new OracleORMHelper();

            OracleCRUDString crudString = new OracleCRUDString(helper);
            TestClass t = new TestClass();
            string expect = "SELECT * FROM TableNm WHERE 1=1 AND TableNm.Id < 1 ";
            var actual = crudString.GetListString(t, new ColumnInfo("Id", "<", "1"));

            Assert.Equal(expect, actual);

            string expect2 = "SELECT * FROM TableNm WHERE 1=1 AND TableNm.Id >= 6 AND TableNm.Data BETWEEN 1 AND 8 ";
            var actual2 = crudString.GetListString(t, new ColumnInfo(nameof(t.Id), ">=", "6"), new ColumnInfo(nameof(t.Data), "between", "1", "8"));

            Assert.Equal(expect2, actual2);

            string expect3 = "SELECT * FROM TableNm WHERE 1=1 AND TableNm.RealColumnName IS NOT NULL ";
            var actual3 = crudString.GetListString(t, new ColumnInfo(helper.ColumnName(t, nameof(t.FakeNameColumn)), "is not null"));

            Assert.Equal(expect3, actual3);


        }


    }

    [Table("TableNm")]
    public class TestClass
    {
        [KeyColumn]
        public int? Id { get; set; }

        public string Data { get; set; }

        [IgnoreColumn]
        public string IgnoreData { get; set; }

        [BindingColumn("RealColumnName")]
        public string FakeNameColumn { get; set; }

        [CreatedDate]
        public DateTime? CDate { get; set; }

        [LastModifiedDate]
        public DateTime? LDate { get; set; }

    }
}