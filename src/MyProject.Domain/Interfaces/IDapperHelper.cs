using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace MyProject.Domain.Interfaces;

public interface IDapperHelper
{
    DbConnection GetDbconnection();

    // Execute Methods
    Task<int> ExecuteAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    Task<int> ExecuteWithTransactionAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    Task<int> ExecuteNonQueryAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    // Query Methods (Single Row)
    Task<T?> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    Task<T?> GetSingleRowAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    Task<T?> GetFirstRowAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    // Query Methods (Multiple Rows)
    Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms = null,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> GetMultipleAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    // Scalar Methods
    Task<T?> GetScalarAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    Task<T?> GetColumnAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    // Insert/Update Methods
    Task<T?> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);
    Task<T?> UpdateAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    // Multiple Result Sets
    Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> GetMultipleResultAsync<T1, T2>(
        string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>> GetMultipleResultAsync<T1, T2, T3>(
        string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>> GetMultipleResultAsync<T1, T2, T3,
        T4>(
        string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    // DataTable Methods
    Task<DataTable> GetDataTableAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    Task<List<DataTable>> GetMultipleDataTablesAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

    // Bulk Operations
    Task<int> BulkInsertAsync<T>(string tableName, IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class;
}