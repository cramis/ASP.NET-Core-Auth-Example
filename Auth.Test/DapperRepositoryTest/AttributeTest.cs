using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using Xunit;

using DapperRepository;
using DapperRepositoryException;

namespace DapperRepository.Test
{
    public class AttributeTest
    {
        [Fact]
        public void Table_어튜리뷰트_테이블_이름_가져오기()
        {

            OracleORMHelper helper = new OracleORMHelper();
            TestClass t = new TestClass();
            string expect = "Test";
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
    }
}