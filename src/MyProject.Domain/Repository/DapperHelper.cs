using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using MyProject.Domain.Interfaces;

namespace MyProject.Domain.Repository;

public class DapperHelper(string connectionString) : IDapperHelper
{
    private readonly int _timeout = 120;

    public DbConnection GetDbconnection()
    {
        return new SqlConnection(connectionString);
    }

    #region Bulk Operations

    public async Task<int> BulkInsertAsync<T>(string tableName, IEnumerable<T> entities,
        CancellationToken cancellationToken = default) where T : class
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition($"INSERT INTO {tableName} SELECT * FROM @entities", new { entities },
            commandTimeout: 0, cancellationToken: cancellationToken);
        return await connection.ExecuteAsync(command);
    }

    #endregion

    #region Execute Methods

    public async Task<int> ExecuteAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        return await connection.ExecuteAsync(command);
    }

    public async Task<int> ExecuteWithTransactionAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            var command = new CommandDefinition(sp, parms, transaction, commandType: commandType, commandTimeout: 0,
                cancellationToken: cancellationToken);
            var result = await connection.ExecuteAsync(command);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<int> ExecuteNonQueryAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        return await connection.ExecuteAsync(command);
    }

    #endregion

    #region Query Methods (Single Row)

    public async Task<T?> GetAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<T>(command);
    }

    public async Task<T?> GetSingleRowAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<T>(command);
    }

    public async Task<T?> GetFirstRowAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<T>(command);
    }

    #endregion

    #region Query Methods (Multiple Rows)

    public async Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms = null,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<T>(command);
        return result.ToList();
    }

    public async Task<IEnumerable<T>> GetMultipleAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        return await connection.QueryAsync<T>(command);
    }

    #endregion

    #region Scalar Methods

    public async Task<T?> GetScalarAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<T>(command);
    }

    public async Task<T?> GetColumnAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<T>(command);
    }

    #endregion

    #region Insert/Update Methods

    public async Task<T?> InsertAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<T>(command);
    }

    public async Task<T?> UpdateAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<T>(command);
    }

    #endregion

    #region Multiple Result Sets

    public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> GetMultipleResultAsync<T1, T2>(string sp,
        DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure,
        CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(command);
        var result1 = await multi.ReadAsync<T1>();
        var result2 = await multi.ReadAsync<T2>();
        return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(result1, result2);
    }

    public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>> GetMultipleResultAsync<T1, T2, T3>(
        string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure,
        CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(command);
        var result1 = await multi.ReadAsync<T1>();
        var result2 = await multi.ReadAsync<T2>();
        var result3 = await multi.ReadAsync<T3>();
        return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(result1, result2, result3);
    }

    public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>>
        GetMultipleResultAsync<T1, T2, T3, T4>(string sp, DynamicParameters parms,
            CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(command);
        var result1 = await multi.ReadAsync<T1>();
        var result2 = await multi.ReadAsync<T2>();
        var result3 = await multi.ReadAsync<T3>();
        var result4 = await multi.ReadAsync<T4>();
        return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>(result1, result2, result3,
            result4);
    }

    #endregion

    #region DataTable Methods

    public async Task<DataTable> GetDataTableAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        await using var reader = await connection.ExecuteReaderAsync(command);
        var dataTable = new DataTable();
        dataTable.Load(reader);
        return dataTable;
    }

    public async Task<List<DataTable>> GetMultipleDataTablesAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
    {
        await using var connection = GetDbconnection();
        var command = new CommandDefinition(sp, parms, commandType: commandType, commandTimeout: 0,
            cancellationToken: cancellationToken);
        await using var reader = await connection.ExecuteReaderAsync(command);
        var dataTables = new List<DataTable>();
        do
        {
            var dataTable = new DataTable();
            dataTable.Load(reader);
            dataTables.Add(dataTable);
        } while (!reader.IsClosed && await reader.NextResultAsync(cancellationToken));

        return dataTables;
    }

    #endregion
}