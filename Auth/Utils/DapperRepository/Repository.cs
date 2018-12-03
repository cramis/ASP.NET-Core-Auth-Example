using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DapperRepository
{
    public interface IDapperRepository
    {
        // GetList
        List<T> GetList<T>(T model);
        Task<List<T>> GetListAsync<T>(T model);
        List<T> GetList<T>(T model, params ParamColumn[] args);
        Task<List<T>> GetListAsync<T>(T model, params ParamColumn[] args);

        //GetItem
        T GetItem<T>(T model);
        Task<T> GetItemAsync<T>(T model);
        T GetItem<T>(T model, params ParamColumn[] args);
        Task<T> GetItemAsync<T>(T model, params ParamColumn[] args);

        //Insert
        int Insert<T>(T model);
        Task<int> InsertAsync<T>(T model);

        //Update
        int Update<T>(T model);
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
            var list = connection.Query<T>(repoString.SelectString(model), model).ToList();

            return list;
        }


        public async Task<List<T>> GetListAsync<T>(T model)
        {
            var list = await connection.QueryAsync<T>(repoString.SelectString(model), model);

            return list.ToList();
        }

        public List<T> GetList<T>(T model, params ParamColumn[] args)
        {
            var list = connection.Query<T>(repoString.SelectString(model, args), model).ToList();

            return list;
        }

        public async Task<List<T>> GetListAsync<T>(T model, params ParamColumn[] args)
        {
            var list = await connection.QueryAsync<T>(repoString.SelectString(model, args), model);

            return list.ToList();
        }

        // GetItem

        public T GetItem<T>(T model)
        {
            var item = connection.QuerySingleOrDefault<T>(repoString.SelectString(model), model);

            return item;
        }

        public async Task<T> GetItemAsync<T>(T model)
        {
            var item = await connection.QuerySingleOrDefaultAsync<T>(repoString.SelectString(model), model);

            return item;
        }
        public T GetItem<T>(T model, params ParamColumn[] args)
        {
            var item = connection.QuerySingleOrDefault<T>(repoString.SelectString(model, args), model);

            return item;
        }

        public async Task<T> GetItemAsync<T>(T model, params ParamColumn[] args)
        {
            var item = await connection.QuerySingleOrDefaultAsync<T>(repoString.SelectString(model, args), model);

            return item;
        }

        // Insert
        public int Insert<T>(T model)
        {
            try
            {
                var result = connection.Execute(repoString.InsertStr(model), model);
                logger.LogDebug(this.LogMessage(repoString.InsertStr(model), JsonConvert.SerializeObject(model)));
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(this.LogMessage(repoString.InsertStr(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        public async Task<int> InsertAsync<T>(T model)
        {
            try
            {
                var result = await connection.ExecuteAsync(repoString.InsertStr(model), model);
                logger.LogDebug(this.LogMessage(repoString.InsertStr(model), JsonConvert.SerializeObject(model)));
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(this.LogMessage(repoString.InsertStr(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        // Update

        public int Update<T>(T model)
        {
            try
            {
                var result = connection.Execute(repoString.UpdateStr(model), model);
                logger.LogDebug(this.LogMessage(repoString.UpdateStr(model), JsonConvert.SerializeObject(model)));
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(this.LogMessage(repoString.UpdateStr(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        public async Task<int> UpdateAsync<T>(T model)
        {
            try
            {
                var result = await connection.ExecuteAsync(repoString.UpdateStr(model), model);
                logger.LogDebug(this.LogMessage(repoString.UpdateStr(model), JsonConvert.SerializeObject(model)));
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(this.LogMessage(repoString.UpdateStr(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        // Merge

        public int Merge<T>(T model)
        {
            try
            {
                var result = connection.Execute(repoString.MergeStr(model), model);
                logger.LogDebug(this.LogMessage(repoString.MergeStr(model), JsonConvert.SerializeObject(model)));

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(this.LogMessage(repoString.MergeStr(model), JsonConvert.SerializeObject(model)));

                throw ex;
            }
        }

        public async Task<int> MergeAsync<T>(T model)
        {
            try
            {
                var result = await connection.ExecuteAsync(repoString.MergeStr(model), model);
                logger.LogDebug(this.LogMessage(repoString.MergeStr(model), JsonConvert.SerializeObject(model)));
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(this.LogMessage(repoString.MergeStr(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        //Delete
        public int Delete<T>(T model)
        {
            try
            {
                var result = connection.Execute(repoString.DeleteStr(model), model);
                logger.LogDebug(this.LogMessage(repoString.DeleteStr(model), JsonConvert.SerializeObject(model)));
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(this.LogMessage(repoString.DeleteStr(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        public async Task<int> DeleteAsync<T>(T model)
        {
            try
            {
                var result = await connection.ExecuteAsync(repoString.DeleteStr(model), model);
                logger.LogDebug(this.LogMessage(repoString.DeleteStr(model), JsonConvert.SerializeObject(model)));
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(this.LogMessage(repoString.DeleteStr(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }


        private string LogMessage(string sql, string args)
        {
            return string.Format("SQL : {0} / Param : {1}", sql, args);
        }
    }
}