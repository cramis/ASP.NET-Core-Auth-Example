using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using Xunit;

using DapperRepository;
using DapperRepositoryException;

namespace DapperRepository.Test
{
    public class OracleRepositoryStringTest
    {
        [Fact]
        public void SelectString_쿼리_가져오기()
        {
            OracleORMHelper helper = new OracleORMHelper();

            OracleRepositoryString RepositoryString = new OracleRepositoryString(helper);
            TestClass t = new TestClass();
            Test2Class t2 = new Test2Class();

            string expect = "SELECT * FROM Test WHERE 1=1";
            var actual = RepositoryString.SelectString(t);

            Assert.Equal(expect, actual);


            t.Id = 1;
            string expect2 = "SELECT * FROM Test WHERE 1=1 AND Test.Id = :Id";
            var actual2 = RepositoryString.SelectString(t);

            Assert.Equal(expect2, actual2);
        }

        [Fact]
        public void SelectString_연산자_이용해서_쿼리_가져오기()
        {
            OracleORMHelper helper = new OracleORMHelper();

            OracleRepositoryString RepositoryString = new OracleRepositoryString(helper);
            TestClass t = new TestClass();
            string expect = "SELECT * FROM Test WHERE 1=1 AND Test.Id < 1";
            var actual = RepositoryString.SelectString(t, new ParamColumn("Id", "<", "1"));

            Assert.Equal(expect, actual);

            string expect2 = "SELECT * FROM Test WHERE 1=1 AND Test.Id >= 6 AND Test.Data BETWEEN 1 AND 8";
            var actual2 = RepositoryString.SelectString(t, new ParamColumn(nameof(t.Id), ">=", "6"), new ParamColumn(nameof(t.Data), "between", "1", "8"));

            Assert.Equal(expect2, actual2);

            string expect3 = "SELECT * FROM Test WHERE 1=1 AND Test.RealColumnName IS NOT NULL AND Test.Data LIKE '%TEST'";
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

            string expect2 = "INSERT INTO Test ( Id, Data, CDate ) VALUES ( :Id, :Data, SYSDATE )";
            var actual2 = RepositoryString.InsertStr(t);

            Assert.Equal(expect2, actual2);


            t.FakeNameColumn = "test";

            string expect3 = "INSERT INTO Test ( Id, Data, RealColumnName, CDate ) VALUES ( :Id, :Data, :FakeNameColumn, SYSDATE )";
            var actual3 = RepositoryString.InsertStr(t);

            Assert.Equal(expect3, actual3);
        }

        [Fact]
        public void InsertString_값_삽입하기_pk_자동생성()
        {
            OracleORMHelper helper = new OracleORMHelper();

            OracleRepositoryString RepositoryString = new OracleRepositoryString(helper);
            Test2Class t2 = new Test2Class();

            Assert.Throws<RequiredValueNotFoundException>(() => RepositoryString.InsertStr(t2));


            t2.Data = "test";

            string expect2 = "INSERT INTO Test2 ( Data, CDate ) VALUES ( :Data, SYSDATE )";
            var actual2 = RepositoryString.InsertStr(t2);

            Assert.Equal(expect2, actual2);


            t2.FakeNameColumn = "test";

            string expect3 = "INSERT INTO Test2 ( Data, RealColumnName, CDate ) VALUES ( :Data, :FakeNameColumn, SYSDATE )";
            var actual3 = RepositoryString.InsertStr(t2);

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

            string expect2 = "UPDATE Test SET Data = :Data, LDate = SYSDATE WHERE Id = :Id";
            var actual2 = RepositoryString.UpdateStr(t);

            Assert.Equal(expect2, actual2);

            string expect3 = "UPDATE Test SET Data = :Data, RealColumnName = NULL, LDate = SYSDATE WHERE Id = :Id";
            var actual3 = RepositoryString.UpdateStr(t, true);

            Assert.Equal(expect3, actual3);

            t.FakeNameColumn = "test";

            string expect4 = "UPDATE Test SET Data = :Data, RealColumnName = :FakeNameColumn, LDate = SYSDATE WHERE Id = :Id";
            var actual4 = RepositoryString.UpdateStr(t);

            Assert.Equal(expect4, actual4);

            // -- Test2

            expect2 = string.Empty;
            expect3 = string.Empty;
            expect4 = string.Empty;
            actual2 = string.Empty;
            actual3 = string.Empty;
            actual4 = string.Empty;


            Test2Class t2 = new Test2Class();
            t2.Data = "test";

            Assert.Throws<PkNotFoundException>(() => RepositoryString.UpdateStr(t2));

            t2.Data = null;
            t2.Id = 1;

            Assert.Throws<RequiredValueNotFoundException>(() => RepositoryString.UpdateStr(t2));

            t2.Data = "test";

            expect2 = "UPDATE Test2 SET Data = :Data, LDate = SYSDATE WHERE Id = :Id";
            actual2 = RepositoryString.UpdateStr(t2);

            Assert.Equal(expect2, actual2);

            expect3 = "UPDATE Test2 SET Data = :Data, RealColumnName = NULL, LDate = SYSDATE WHERE Id = :Id";
            actual3 = RepositoryString.UpdateStr(t2, true);

            Assert.Equal(expect3, actual3);

            t2.FakeNameColumn = "test";

            expect4 = "UPDATE Test2 SET Data = :Data, RealColumnName = :FakeNameColumn, LDate = SYSDATE WHERE Id = :Id";
            actual4 = RepositoryString.UpdateStr(t2);

            Assert.Equal(expect4, actual4);
        }


        [Fact]
        public void MergeString_값_병합하기()
        {
            OracleORMHelper helper = new OracleORMHelper();

            OracleRepositoryString RepositoryString = new OracleRepositoryString(helper);
            TestClass t = new TestClass();

            Assert.Throws<PkNotFoundException>(() => RepositoryString.MergeStr(t));

            t.Id = 1;

            t.Data = "test";

            string expect2 = "MERGE INTO Test USING DUAL ON ( Id = :Id ) WHEN MATCHED THEN UPDATE SET Data = :Data, LDate = SYSDATE WHEN NOT MATCHED THEN INSERT ( Id, Data, CDate ) VALUES ( :Id, :Data, SYSDATE )";
            var actual2 = RepositoryString.MergeStr(t);

            Assert.Equal(expect2, actual2);

            string expect3 = "MERGE INTO Test USING DUAL ON ( Id = :Id ) WHEN MATCHED THEN UPDATE SET Data = :Data, RealColumnName = NULL, LDate = SYSDATE WHEN NOT MATCHED THEN INSERT ( Id, Data, CDate ) VALUES ( :Id, :Data, SYSDATE )";
            var actual3 = RepositoryString.MergeStr(t, true);

            Assert.Equal(expect3, actual3);

            t.FakeNameColumn = "test";

            string expect4 = "MERGE INTO Test USING DUAL ON ( Id = :Id ) WHEN MATCHED THEN UPDATE SET Data = :Data, RealColumnName = :FakeNameColumn, LDate = SYSDATE WHEN NOT MATCHED THEN INSERT ( Id, Data, RealColumnName, CDate ) VALUES ( :Id, :Data, :FakeNameColumn, SYSDATE )";
            var actual4 = RepositoryString.MergeStr(t);

            Assert.Equal(expect4, actual4);
        }


        [Fact]
        public void DeleteString_값_삭제하기()
        {
            OracleORMHelper helper = new OracleORMHelper();

            OracleRepositoryString RepositoryString = new OracleRepositoryString(helper);
            TestClass t = new TestClass();
            string expect = "DELETE FROM Test WHERE 1=1";
            var actual = RepositoryString.DeleteStr(t);

            Assert.Equal(expect, actual);


            t.Id = 1;
            string expect2 = "DELETE FROM Test WHERE 1=1 AND Id = :Id";
            var actual2 = RepositoryString.DeleteStr(t);

            Assert.Equal(expect2, actual2);


            t.Data = "test1";
            string expect3 = "DELETE FROM Test WHERE 1=1 AND Id = :Id AND Data = :Data";
            var actual3 = RepositoryString.DeleteStr(t);

            Assert.Equal(expect3, actual3);
        }

        [Fact]
        public void DeleteStr_연산자_이용해서_값_삭제하기()
        {
            OracleORMHelper helper = new OracleORMHelper();

            OracleRepositoryString RepositoryString = new OracleRepositoryString(helper);
            TestClass t = new TestClass();
            string expect = "DELETE FROM Test WHERE 1=1 AND Id < 1";
            var actual = RepositoryString.DeleteStr(t, new ParamColumn("Id", "<", "1"));

            Assert.Equal(expect, actual);

            string expect2 = "DELETE FROM Test WHERE 1=1 AND Id >= 6 AND Data BETWEEN 1 AND 8";
            var actual2 = RepositoryString.DeleteStr(t, new ParamColumn(nameof(t.Id), ">=", "6"), new ParamColumn(nameof(t.Data), "between", "1", "8"));

            Assert.Equal(expect2, actual2);

            string expect3 = "DELETE FROM Test WHERE 1=1 AND RealColumnName IS NOT NULL AND Data LIKE '%TEST'";
            var actual3 = RepositoryString.DeleteStr(t, new ParamColumn(helper.ColumnName(t, nameof(t.FakeNameColumn)), "is not null"), new ParamColumn(nameof(t.Data), "like", "'%TEST'"));

            Assert.Equal(expect3, actual3);
        }
    }
}