using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;

namespace DapperRepository
{
    public interface IDapperRepository
    {
        void SetConnection(IDbConnection conn);
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

        Serilog.ILogger logger;

        public BaseRepository(IRepositoryString repoString)
        {
            this.repoString = repoString;
            this.logger = Log.Logger;
        }

        public void SetConnection(IDbConnection conn)
        {
            this.connection = conn;
        }

        // GetList
        public List<T> GetList<T>(T model)
        {
            try
            {
                var list = connection.Query<T>(repoString.SelectString(model), model).ToList();
                logger.Debug(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                return list;
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }


        public async Task<List<T>> GetListAsync<T>(T model)
        {
            try
            {
                var list = await connection.QueryAsync<T>(repoString.SelectString(model), model);

                logger.Debug(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                return list.ToList();
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        public List<T> GetList<T>(T model, params ParamColumn[] args)
        {
            try
            {
                var list = connection.Query<T>(repoString.SelectString(model, args), model).ToList();

                logger.Debug(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                return list;
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        public async Task<List<T>> GetListAsync<T>(T model, params ParamColumn[] args)
        {
            try
            {
                var list = await connection.QueryAsync<T>(repoString.SelectString(model, args), model);

                logger.Debug(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                return list.ToList();
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        // GetItem

        public T GetItem<T>(T model)
        {
            try
            {
                var item = connection.QuerySingleOrDefault<T>(repoString.SelectString(model), model);

                logger.Debug(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                return item;
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        public async Task<T> GetItemAsync<T>(T model)
        {
            try
            {
                var item = await connection.QuerySingleOrDefaultAsync<T>(repoString.SelectString(model), model);

                logger.Debug(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                return item;
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }
        public T GetItem<T>(T model, params ParamColumn[] args)
        {
            try
            {
                var item = connection.QuerySingleOrDefault<T>(repoString.SelectString(model, args), model);

                logger.Debug(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                return item;
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        public async Task<T> GetItemAsync<T>(T model, params ParamColumn[] args)
        {
            try
            {
                var item = await connection.QuerySingleOrDefaultAsync<T>(repoString.SelectString(model, args), model);

                logger.Debug(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                return item;
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.SelectString(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        // Insert
        public int Insert<T>(T model)
        {
            try
            {
                var result = connection.Execute(repoString.InsertStr(model), model);
                logger.Debug(this.LogMessage(repoString.InsertStr(model), JsonConvert.SerializeObject(model)));
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.InsertStr(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        public async Task<int> InsertAsync<T>(T model)
        {
            try
            {
                var result = await connection.ExecuteAsync(repoString.InsertStr(model), model);
                logger.Debug(this.LogMessage(repoString.InsertStr(model), JsonConvert.SerializeObject(model)));
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.InsertStr(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        // Update

        public int Update<T>(T model)
        {
            try
            {
                var result = connection.Execute(repoString.UpdateStr(model), model);
                logger.Debug(this.LogMessage(repoString.UpdateStr(model), JsonConvert.SerializeObject(model)));
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.UpdateStr(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        public async Task<int> UpdateAsync<T>(T model)
        {
            try
            {
                var result = await connection.ExecuteAsync(repoString.UpdateStr(model), model);
                logger.Debug(this.LogMessage(repoString.UpdateStr(model), JsonConvert.SerializeObject(model)));
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.UpdateStr(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        // Merge

        public int Merge<T>(T model)
        {
            try
            {
                var result = connection.Execute(repoString.MergeStr(model), model);
                logger.Debug(this.LogMessage(repoString.MergeStr(model), JsonConvert.SerializeObject(model)));

                return result;
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.MergeStr(model), JsonConvert.SerializeObject(model)));

                throw ex;
            }
        }

        public async Task<int> MergeAsync<T>(T model)
        {
            try
            {
                var result = await connection.ExecuteAsync(repoString.MergeStr(model), model);
                logger.Debug(this.LogMessage(repoString.MergeStr(model), JsonConvert.SerializeObject(model)));
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.MergeStr(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        //Delete
        public int Delete<T>(T model)
        {
            try
            {
                var result = connection.Execute(repoString.DeleteStr(model), model);
                logger.Debug(this.LogMessage(repoString.DeleteStr(model), JsonConvert.SerializeObject(model)));
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.DeleteStr(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        public async Task<int> DeleteAsync<T>(T model)
        {
            try
            {
                var result = await connection.ExecuteAsync(repoString.DeleteStr(model), model);
                logger.Debug(this.LogMessage(repoString.DeleteStr(model), JsonConvert.SerializeObject(model)));
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(this.LogMessage(repoString.DeleteStr(model), JsonConvert.SerializeObject(model)));
                throw ex;
            }
        }

        private string LogMessage(string sql, string args)
        {
            return string.Format("SQL : {0} / Param : {1}", sql, args);
        }
    }
}