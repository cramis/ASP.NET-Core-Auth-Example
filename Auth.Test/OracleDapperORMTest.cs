using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using Xunit;

using DapperRepository;
using DapperRepositoryException;

namespace DapperRepository.Test
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
        public void SelectString_쿼리_가져오기()
        {
            OracleORMHelper helper = new OracleORMHelper();

            OracleRepositoryString RepositoryString = new OracleRepositoryString(helper);
            TestClass t = new TestClass();
            string expect = "SELECT * FROM TableNm WHERE 1=1";
            var actual = RepositoryString.SelectString(t);

            Assert.Equal(expect, actual);


            t.Id = 1;
            string expect2 = "SELECT * FROM TableNm WHERE 1=1 AND TableNm.Id = :Id";
            var actual2 = RepositoryString.SelectString(t);

            Assert.Equal(expect2, actual2);
        }

        [Fact]
        public void SelectString_연산자_이용해서_쿼리_가져오기()
        {
            OracleORMHelper helper = new OracleORMHelper();

            OracleRepositoryString RepositoryString = new OracleRepositoryString(helper);
            TestClass t = new TestClass();
            string expect = "SELECT * FROM TableNm WHERE 1=1 AND TableNm.Id < 1";
            var actual = RepositoryString.SelectString(t, new ParamColumn("Id", "<", "1"));

            Assert.Equal(expect, actual);

            string expect2 = "SELECT * FROM TableNm WHERE 1=1 AND TableNm.Id >= 6 AND TableNm.Data BETWEEN 1 AND 8";
            var actual2 = RepositoryString.SelectString(t, new ParamColumn(nameof(t.Id), ">=", "6"), new ParamColumn(nameof(t.Data), "between", "1", "8"));

            Assert.Equal(expect2, actual2);

            string expect3 = "SELECT * FROM TableNm WHERE 1=1 AND TableNm.RealColumnName IS NOT NULL AND TableNm.Data LIKE '%TEST'";
            var actual3 = RepositoryString.SelectString(t, new ParamColumn(helper.ColumnName(t, nameof(t.FakeNameColumn)), "is not null"), new ParamColumn(nameof(t.Data), "like", "'%TEST'"));

            Assert.Equal(expect3, actual3);
        }

        [Fact]
        public void InsertString_값_삽입하기()
        {
            OracleORMHelper helper = new OracleORMHelper();

            OracleRepositoryString RepositoryString = new OracleRepositoryString(helper);
            TestClass t = new TestClass();

            Assert.Throws<PkNotFoundException>(() => RepositoryString.InsertStr(t));

            t.Id = 1;
            t.Data = "test";

            string expect2 = "INSERT INTO TableNm ( Id, Data, CDate ) VALUES ( :Id, :Data, sysdate )";
            var actual2 = RepositoryString.InsertStr(t);

            Assert.Equal(expect2, actual2);


            t.FakeNameColumn = "test";

            string expect3 = "INSERT INTO TableNm ( Id, Data, RealColumnName, CDate ) VALUES ( :Id, :Data, :FakeNameColumn, sysdate )";
            var actual3 = RepositoryString.InsertStr(t);

            Assert.Equal(expect3, actual3);
        }

        [Fact]
        public void UpdateString_값_수정하기()
        {
            OracleORMHelper helper = new OracleORMHelper();

            OracleRepositoryString RepositoryString = new OracleRepositoryString(helper);
            TestClass t = new TestClass();

            Assert.Throws<PkNotFoundException>(() => RepositoryString.UpdateStr(t));

            t.Id = 1;

            t.Data = "test";

            string expect2 = "UPDATE TableNm SET Id = :Id, Data = :Data, LDate = sysdate WHERE Id = :Id";
            var actual2 = RepositoryString.UpdateStr(t);

            Assert.Equal(expect2, actual2);

            string expect3 = "UPDATE TableNm SET Id = :Id, Data = :Data, RealColumnName = NULL, LDate = sysdate WHERE Id = :Id";
            var actual3 = RepositoryString.UpdateStr(t, true);

            Assert.Equal(expect3, actual3);

            t.FakeNameColumn = "test";

            string expect4 = "UPDATE TableNm SET Id = :Id, Data = :Data, RealColumnName = :FakeNameColumn, LDate = sysdate WHERE Id = :Id";
            var actual4 = RepositoryString.UpdateStr(t);

            Assert.Equal(expect4, actual4);
        }


        [Fact]
        public void DeleteString_값_삭제하기()
        {
            OracleORMHelper helper = new OracleORMHelper();

            OracleRepositoryString RepositoryString = new OracleRepositoryString(helper);
            TestClass t = new TestClass();
            string expect = "DELETE FROM TableNm WHERE 1=1";
            var actual = RepositoryString.DeleteStr(t);

            Assert.Equal(expect, actual);


            t.Id = 1;
            string expect2 = "DELETE FROM TableNm WHERE 1=1 AND TableNm.Id = :Id";
            var actual2 = RepositoryString.DeleteStr(t);

            Assert.Equal(expect2, actual2);
        }

        [Fact]
        public void DeleteStr_연산자_이용해서_값_삭제하기()
        {
            OracleORMHelper helper = new OracleORMHelper();

            OracleRepositoryString RepositoryString = new OracleRepositoryString(helper);
            TestClass t = new TestClass();
            string expect = "DELETE FROM TableNm WHERE 1=1 AND TableNm.Id < 1";
            var actual = RepositoryString.DeleteStr(t, new ParamColumn("Id", "<", "1"));

            Assert.Equal(expect, actual);

            string expect2 = "DELETE FROM TableNm WHERE 1=1 AND TableNm.Id >= 6 AND TableNm.Data BETWEEN 1 AND 8";
            var actual2 = RepositoryString.DeleteStr(t, new ParamColumn(nameof(t.Id), ">=", "6"), new ParamColumn(nameof(t.Data), "between", "1", "8"));

            Assert.Equal(expect2, actual2);

            string expect3 = "DELETE FROM TableNm WHERE 1=1 AND TableNm.RealColumnName IS NOT NULL AND TableNm.Data LIKE '%TEST'";
            var actual3 = RepositoryString.DeleteStr(t, new ParamColumn(helper.ColumnName(t, nameof(t.FakeNameColumn)), "is not null"), new ParamColumn(nameof(t.Data), "like", "'%TEST'"));

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