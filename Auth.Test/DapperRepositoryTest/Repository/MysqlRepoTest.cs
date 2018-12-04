using System;
using Auth.Entities;
using Auth.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using Xunit;

using DapperRepository;
using System.Data.SQLite;
using System.IO;
using Microsoft.Extensions.Logging;
using Moq;
using System.Transactions;

namespace DapperRepository.Test
{
    public class MysqlRepoTest
    {
        private IORMHelper helper;
        private IRepositoryString repoString;
        private IDapperRepository repo;

        private ILogger logger;


        public MysqlRepoTest()
        {

            this.helper = new BaseORMHelper();

            this.repoString = new MysqlRepositoryString(helper);




            this.repo = new BaseRepository(repoString);

            this.repo.SetConnection(new ConnectionFactory().Connection("mysql"));
        }

        [Fact]
        public void Test()
        {

            TestClass t1 = new TestClass();

            t1.Id = 1;
            t1.Data = "test1";

            TestClass t2 = new TestClass();

            t2.Id = 2;
            t2.Data = "test2";
            t2.FakeNameColumn = "2222";

            var result = repo.Insert(t1);
            Assert.Equal(1, result);

            result = repo.Insert(t2);
            Assert.Equal(1, result);

            var item1 = repo.GetItem(new TestClass() { Id = 1 });
            Assert.Equal("test1", item1.Data);

            var item2 = repo.GetItem(new TestClass() { Id = 2 });
            Assert.Equal("test2", item2.Data);

            // update 테스트
            t1.Data = "test1-1";
            t1.FakeNameColumn = "1111";

            result = repo.Update(t1);

            Assert.Equal(1, result);

            item1 = repo.GetItem(new TestClass() { Id = 1 });
            Assert.Equal("test1-1", item1.Data);

            // 조건 리스트
            var list = repo.GetList(t1, new ParamColumn("Id", "<", "3"));

            Assert.Equal(2, list.Count);

            // 조건 아이템

            var item4 = repo.GetItem(t1, new ParamColumn(nameof(t1.LDate), "is not null", ""));

            Assert.Equal(1, item4.Id);

            item4 = repo.GetItem(t1, new ParamColumn(nameof(t1.LDate), "is null", null));

            Assert.Equal(2, item4.Id);

            item4 = repo.GetItem(t1, new ParamColumn(nameof(t1.Data), "like", "'%1-1'"));

            Assert.Equal(1, item4.Id);

            // Delete
            result = repo.Delete(new TestClass() { Id = 1, Data = "test1-1" });

            Assert.Equal(1, result);

            result = repo.Delete(new TestClass() { Id = 2 });

            Assert.Equal(1, result);

        }

        [Fact]
        public void Merge_test()
        {

            TestClass t1 = new TestClass();

            t1.Id = 3;
            t1.Data = "test3";

            var result = repo.Merge(t1);
            Assert.Equal(1, result);


            t1.Id = 3;
            t1.Data = "test4";
            result = repo.Merge(t1);

            Assert.Equal(1, result);

            result = repo.Delete(new TestClass() { Id = 3 });

            Assert.Equal(1, result);
        }
    }
}