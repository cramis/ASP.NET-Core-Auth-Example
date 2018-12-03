using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;

namespace DapperRepository
{
    public interface IDapperRepository
    {
        // GetList
        List<T> GetList<T>(T model);
        Task<List<T>> GetListAsync<T>(T model);

        //GetItem
        T GetItem<T>(T model);
        Task<T> GetItemAsync<T>(T model);

        //Insert
        int Insert<T>(T model);
        Task<int> InsertAsync<T>(T model);

        //Update
        int Update<T>(object model);
        Task<int> UpdateAsync<T>(T model);


        // Merge        T Merge<T>();
        int Merge<T>(T model);
        Task<int> MergeAsync<T>(T model);

        // Delete
        int Delete<T>(T model);
        Task<int> DeleteAsync<T>(T model);


    }
    public class BaseRepository : IDapperRepository
    {
        private IDbConnection connection;
        private object Model;

        private IRepositoryString repoString;

        ILogger logger;


        public BaseRepository(IDbConnection conn, IRepositoryString repoString, ILogger logger)
        {
            this.connection = conn;
            this.repoString = repoString;
            this.logger = logger;
        }

        // GetList
        public List<T> GetList<T>(T model)
        {
            var list = connection.Query<T>(repoString.SelectString(model).ToString(), model).ToList();

            return list;
        }


        public async Task<List<T>> GetListAsync<T>(T model)
        {
            var list = await connection.QueryAsync<T>(repoString.SelectString(model).ToString(), model);

            return list.ToList();
        }

        // GetItem

        public T GetItem<T>(T model)
        {
            var item = connection.QuerySingleOrDefault<T>(repoString.SelectString(model).ToString(), model);

            return item;
        }

        public async Task<T> GetItemAsync<T>(T model)
        {
            var item = await connection.QuerySingleOrDefaultAsync<T>(repoString.SelectString(model).ToString(), model);

            return item;
        }

        // Insert
        public int Insert<T>(T model)
        {
            try
            {
                var result = connection.Execute(repoString.InsertStr(model).ToString(), model);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> InsertAsync<T>(T model)
        {
            try
            {
                var result = await connection.ExecuteAsync(repoString.InsertStr(model).ToString(), model);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Update

        public int Update<T>(object model)
        {
            try
            {
                var result = connection.Execute(repoString.UpdateStr(model).ToString(), model);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateAsync<T>(T model)
        {
            try
            {
                var result = await connection.ExecuteAsync(repoString.UpdateStr(model).ToString(), model);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Merge

        public int Merge<T>(T model)
        {
            try
            {
                var result = connection.Execute(repoString.MergeStr(model).ToString(), model);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> MergeAsync<T>(T model)
        {
            try
            {
                var result = await connection.ExecuteAsync(repoString.MergeStr(model).ToString(), model);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete
        public int Delete<T>(T model)
        {
            try
            {
                var result = connection.Execute(repoString.DeleteStr(model).ToString(), model);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteAsync<T>(T model)
        {
            try
            {
                var result = await connection.ExecuteAsync(repoString.DeleteStr(model).ToString(), model);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}