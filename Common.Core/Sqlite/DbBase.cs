using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLite;

namespace Common.Core
{
    public class DbBase
    {
        protected SQLiteAsyncConnection conn;

        public SQLiteAsyncConnection Connection
        {
            get { return conn; }
        }

        public DbBase() : this("appSqlite.db")
        {
        }
        public DbBase(string dbName)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            conn = new SQLiteAsyncConnection(System.IO.Path.Combine(folder, dbName));
        }

        public virtual async Task<bool> Init() { return await Task.Run(() => { return true; }); }

        public async Task<GenericResponse<List<T>>> GetAll<T>(bool includeDeleted = false) where T : DataModel, new()
        {
            var response = new GenericResponse<List<T>>();
            try
            {
                var query = conn.Table<T>();
                if (!includeDeleted)
                    query = query.Where(x => x.MarkedForDelete == false);
                response.Response = await query.ToListAsync();
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Error = ex;
                return response;
            }
        }

        public async Task<BooleanResponse> TruncateAsync<T>() where T : ObservableObject, new()
        {
            var response = new BooleanResponse();
            await conn.RunInTransactionAsync(async (tran) =>
            {
                try
                {
                    await conn.DropTableAsync<T>();
                    await conn.CreateTableAsync<T>();
                    tran.Commit();
                    response.Success = true;
                }
                catch (Exception ex)
                {
                    response.Error = ex;
                    tran.Rollback();
                }
            });
            return response;
        }

        public async Task<GenericResponse<T>> GetByInternalId<T>(Guid CorrelationID, bool includeDeleted = false) where T : DataModel, new()
        {
            var response = new GenericResponse<T>();
            try
            {
                var query = conn.Table<T>().Where(x => x.CorrelationID == CorrelationID);
                if (!includeDeleted)
                    query = query.Where(x => x.MarkedForDelete == false);

                response.Response = await query.FirstOrDefaultAsync();
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Error = ex;
                return response;
            }


        }
        public async Task<GenericResponse<List<T>>> GetByQuery<T>(Expression<Func<T, bool>> exp, bool includeDeleted = false) where T : DataModel, new()
        {
            var response = new GenericResponse<List<T>>();
            try
            {
                var query = conn.Table<T>().Where(exp);
                if (!includeDeleted)
                    query = query.Where(x => x.MarkedForDelete == false);
                response.Response = await query.ToListAsync();
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Error = ex;
                return response;
            }

        }

        public BooleanResponse AddOrUpdate<T, P>(T obj, Expression<Func<T, P>> exp, SQLiteConnection transaction) where T : DataModel, new()
        {
            var response = new BooleanResponse();
            try
            {
                int rowsAffected = 0;
                obj.TimeStamp = DateTime.Now;
                var expression = (MemberExpression)exp.Body;
                string name = expression.Member.Name;
                var prop = obj.GetType().GetProperty(name);
                var vObj = (P)prop.GetValue(obj, null);

                if (default(P).Equals(vObj))
                {
                    throw new ApplicationException($"Instance of model is missing primary key identified for {typeof(T).Name}");
                }
                else {
                    var stmt = $"SELECT count({name}) FROM {obj.GetType().Name} WHERE {name} = ?";
                    var result = transaction.ExecuteScalar<int>(stmt, vObj);
                    if (result > 0)
                    {
                        var existingGuid = $"SELECT InternalID FROM {obj.GetType().Name} WHERE {name} = ?";
                        var guidResult = transaction.ExecuteScalar<Guid>(existingGuid, vObj);
                        obj.CorrelationID = guidResult;
                        rowsAffected = transaction.Update(obj);
                    }
                    else {
                        if (obj.CorrelationID == default(Guid))
                        {
                            obj.CorrelationID = Guid.NewGuid();
                        }
                        rowsAffected = transaction.Insert(obj);
                    }
                }
                response.Success = rowsAffected == 1 ? true : false;
                return response;
            }
            catch (Exception ex)
            {
                response.Error = ex;
                return response;
            }
        }

        public async Task<BooleanResponse> AddOrUpdate<T, P>(T obj, Expression<Func<T, P>> exp) where T : DataModel, new()
        {
            var response = new BooleanResponse();

            int rowsAffected = 0;
            obj.TimeStamp = DateTime.Now;
            try
            {
                var expression = (MemberExpression)exp.Body;
                string name = expression.Member.Name;
                var prop = obj.GetType().GetProperty(name);
                var vObj = (P)prop.GetValue(obj, null);

                if (default(P).Equals(vObj))
                {
                    throw new ApplicationException($"Instance of model is missing primary key identified for {typeof(T).Name}");
                }
                else {
                    var stmt = $"SELECT count({name}) FROM {obj.GetType().Name} WHERE {name} = ?";
                    var result = await conn.ExecuteScalarAsync<int>(stmt, vObj);
                    if (result > 0)
                    {
                        var existingGuid = $"SELECT InternalID FROM {obj.GetType().Name} WHERE {name} = ?";
                        var guidResult = await conn.ExecuteScalarAsync<Guid>(existingGuid, vObj);
                        obj.CorrelationID = guidResult;
                        rowsAffected = await conn.UpdateAsync(obj);
                    }
                    else {
                        if (obj.CorrelationID == default(Guid))
                        {
                            obj.CorrelationID = Guid.NewGuid();
                        }
                        rowsAffected = await conn.InsertAsync(obj);
                    }
                }
                response.Success = rowsAffected == 1 ? true : false;
                return response;

            }
            catch (Exception ex)
            {
                response.Error = ex;
                return response;
            }

        }

        public async Task<BooleanResponse> AddOrUpdate<T>(T obj) where T : DataModel, new()
        {
            var response = new BooleanResponse();

            int rowsAffected = 0;
            obj.TimeStamp = DateTime.Now;
            try
            {
                if (obj.CorrelationID != default(Guid))
                {
                    rowsAffected = await conn.UpdateAsync(obj);
                }
                else {
                    obj.CorrelationID = Guid.NewGuid();
                    rowsAffected = await conn.InsertAsync(obj);
                    if (rowsAffected != 1)
                        obj.CorrelationID = default(Guid);
                }

                response.Success = rowsAffected == 1 ? true : false;
                return response;
            }
            catch (Exception ex)
            {
                response.Error = ex;
                return response;
            }

        }

        public async Task<BooleanResponse> AddOrUpdateAll<T, P>(List<T> collection, Expression<Func<T, P>> exp) where T : DataModel, new()
        {
            var st = DateTime.Now;

            var response = new BooleanResponse();
            collection.ForEach((obj) => obj.TimeStamp = DateTime.Now);
            try
            {
                var expression = (MemberExpression)exp.Body;
                string name = expression.Member.Name;
                var prop = typeof(T).GetProperty(name);

                var inserts = new List<T>();
                var updates = new List<T>();

                foreach (var obj in collection)
                {
                    var vObj = (P)prop.GetValue(obj, null);
                    if (!default(P).Equals(vObj))
                    {
                        var stmt = $"SELECT count({name}) FROM {obj.GetType().Name} WHERE {name} = ?";
                        var result = await conn.ExecuteScalarAsync<int>(stmt, vObj);
                        if (result > 0)
                        {
                            var existingGuid = $"SELECT InternalID FROM {obj.GetType().Name} WHERE {name} = ?";
                            var guidResult = await conn.ExecuteScalarAsync<Guid>(existingGuid, vObj);
                            obj.CorrelationID = guidResult;
                            updates.Add(obj);
                        }
                        else
                        {
                            if (obj.CorrelationID == default(Guid))
                            {
                                obj.CorrelationID = Guid.NewGuid();
                            }
                            inserts.Add(obj);
                        }
                    }
                }

                var totalInserted = await conn.InsertAllAsync(inserts);
                var totalUpdated = await conn.UpdateAllAsync(updates);

                if (totalUpdated == updates.Count && totalInserted == inserts.Count)
                    response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Error = ex;
                return response;
            }

            var et = DateTime.Now;
            var sp = et - st;
            var total = (sp.TotalSeconds * 1000) + sp.TotalMilliseconds;
            Console.WriteLine($"Total ms for FSDB write is {total} on {typeof(T).Name}");

        }

        public async Task<BooleanResponse> DeleteByInternalID<T>(Guid ID, bool softDelete = true) where T : DataModel, new()
        {
            var response = new BooleanResponse();
            try
            {
                var obj = await GetByInternalId<T>(ID);
                int rowsAffected = 0;
                if (obj.Success)
                {
                    if (softDelete)
                    {
                        obj.Response.TimeStamp = DateTime.Now;
                        obj.Response.MarkedForDelete = softDelete;
                        rowsAffected = await conn.UpdateAsync(obj.Response);
                    }
                    else {
                        rowsAffected = await conn.DeleteAsync(obj.Response);
                    }
                    response.Success = rowsAffected == 1 ? true : false;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Error = ex;
                return response;
            }

        }
        public async Task<BooleanResponse> DeleteByQuery<T>(Expression<Func<T, bool>> exp, bool softDelete = true) where T : DataModel, new()
        {
            var response = new BooleanResponse();
            try
            {
                int rowsAffected = 0;
                var obj = await conn.Table<T>().Where(exp).FirstOrDefaultAsync();
                if (obj != null)
                {
                    if (softDelete)
                    {
                        obj.TimeStamp = DateTime.Now;
                        obj.MarkedForDelete = softDelete;
                        rowsAffected = await conn.UpdateAsync(obj);
                    }
                    else {
                        rowsAffected = await conn.DeleteAsync(obj);
                    }
                    response.Success = rowsAffected == 1 ? true : false;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Error = ex;
                return response;
            }
        }

    }

    //public class TranDemo{

    //    public DbManager Db{
    //        get { return DbManager.Instance;}
    //    }
    //    public SQLiteAsyncConnection Conn{
    //        get { return DbManager.Instance.Connection;}
    //    }notepad

    //    public async Task UpdateMultipleViaTransaction(){
    //        await Conn.RunInTransactionAsync( (SQLiteConnection tran) =>
    //        {
    //            try
    //            {
    //                tran.Insert(new PersonTest() { FirstName = "Sam", LastName = "Houston" });
    //                tran.Insert(new PersonTest() { FirstName = "Buck", LastName = "Rodgers" });
    //                tran.Commit();
    //            }
    //            catch(Exception ex){
    //                tran.Rollback();
    //            }
    //        });
    //    }
    //}
}

