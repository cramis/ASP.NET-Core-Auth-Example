using System;
using Auth.Entities;
using Auth.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using Xunit;

using DapperRepository;
using System.Data.SQLite;
using System.IO;

namespace DapperRepository.Test
{
    public class SqliteRepoTest
    {
        public static string DbFile
        {
            // get { return Environment.CurrentDirectory + "\\SimpleDb.sqlite"; }
            get { return Directory.GetCurrentDirectory() + "\\SimpleDb.sqlite"; }

        }

        public static SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile);
        }

        private IORMHelper helper;
        private IRepositoryString repoString;
        private IDapperRepository repo;


        public SqliteRepoTest()
        {
            this.helper = new BaseORMHelper();

            this.repoString = new SqliteRepositoryString(helper);

            this.repo = new BaseRepository(SimpleDbConnection(), repoString, null);
        }

        [Fact]
        public void Test()
        {

            TestClass t = new TestClass();

            t.Id = 2;
            t.Data = "test2";

            var result = repo.Insert(t);

            Assert.Equal(1, result);
        }
    }
}