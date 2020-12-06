using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;


namespace CoreFlogger.Entity
{
    public class FloggerEFInterceptor : DbCommandInterceptor
    {
        private Exception WrapEntityFrameworkException(DbCommand command, Exception ex)
        {
            var newException = new Exception("EntityFramework command failed!", ex);
            AddParamsToException(command.Parameters, newException);
            return newException;
        }

        private void AddParamsToException(DbParameterCollection parameters, Exception exception)
        {
            foreach (DbParameter param in parameters)
            {
                exception.Data.Add(param.ParameterName, param.Value.ToString());
            }
        }
        public InterceptionResult ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult result)
        {
            // Manipulate the command text, etc. here...
            command.CommandText += " OPTION (OPTIMIZE FOR UNKNOWN)";
            return result;
        }
        //public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        //{
        //    if (interceptionContext.Exception != null)
        //        interceptionContext.Exception = WrapEntityFrameworkException(command, interceptionContext.Exception);
        //}

        //public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        //{
        //}

        //public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        //{
        //    if (interceptionContext.Exception != null)
        //        interceptionContext.Exception = WrapEntityFrameworkException(command, interceptionContext.Exception);
        //}

        //public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        //{
        //}

        //public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        //{
        //    if (interceptionContext.Exception != null)
        //        interceptionContext.Exception = WrapEntityFrameworkException(command, interceptionContext.Exception);
        //}

        //public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        //{
        //}

        public InterceptionResult CommandCreating(
                DbCommand command,
                CommandEventData eventData,
                InterceptionResult result)
        {
            // Manipulate the command text, etc. here...
            command.CommandText += " OPTION (OPTIMIZE FOR UNKNOWN)";


            return result;
        }



        //public DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
        //{
        //    throw new NotImplementedException();
        //}

        //public InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        //{
        //    throw new NotImplementedException();
        //}

        //public InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
        //{
        //    throw new NotImplementedException();
        //}

        //public InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
        //{
        //    throw new NotImplementedException();
        //}

        //public async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result,
        //    CancellationToken cancellationToken = new CancellationToken())
        //{
        //     throw new NotImplementedException();
        //}

        //public async ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result,
        //    CancellationToken cancellationToken = new CancellationToken())
        //{
        //    throw new NotImplementedException();
        //}

        //public async ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result,
        //    CancellationToken cancellationToken = new CancellationToken())
        //{
        //    throw new NotImplementedException();
        //}

        //public DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
        //{
        //    throw new NotImplementedException();
        //}

        //public object ScalarExecuted(DbCommand command, CommandExecutedEventData eventData, object result)
        //{
        //    throw new NotImplementedException();
        //}

        //public int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
        //{
        //    throw new NotImplementedException();
        //}

        //public async ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result,
        //    CancellationToken cancellationToken = new CancellationToken())
        //{
        //    throw new NotImplementedException();
        //}

        //public async ValueTask<object> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object result,
        //    CancellationToken cancellationToken = new CancellationToken())
        //{
        //    throw new NotImplementedException();
        //}

        //public async ValueTask<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result,
        //    CancellationToken cancellationToken = new CancellationToken())
        //{
        //    throw new NotImplementedException();
        //}

        public void CommandFailed(DbCommand command, CommandErrorEventData eventData)
        {
            throw new NotImplementedException();
        }

        public async Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public InterceptionResult DataReaderDisposing(DbCommand command, DataReaderDisposingEventData eventData,
            InterceptionResult result)
        {
            throw new NotImplementedException();
        }
    }
}
