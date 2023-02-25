using Dapper;
using MisGastosApi.Data.Models;
using System.Data;

namespace MisGastosApi.Data.Repositories
{
    public class PaymentMethodRepository: IPaymentMethodRepository
    {
        private readonly ILogger<PaymentMethodRepository> logger;
        private readonly IDbConnection dbConnection;

        public PaymentMethodRepository(ILogger<PaymentMethodRepository> logger, DapperContext context)
        {
            this.logger = logger;
            dbConnection = context.CreateConnection();
        }

        private IDbTransaction CreateTransaction()
        {
            return dbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public async Task<List<PaymentMethod>> GetPaymentsByUser(string userId)
        {
            var query = @"SELECT	PaymentMethodId,
		                        UserId,
		                        Description,
		                        DeadLine,
		                        CreatedDate,
		                        UpdatedDate
                        FROM PaymentMethod
                        WHERE UserId = @userId
                        Order by Description";

            try
            {
                return (await dbConnection.QueryAsync<PaymentMethod>(query, new { userId })).ToList();
            }
            catch (Exception e)
            {
                this.logger.LogError(query, e.Message);
                throw;
            }
        }

        public async Task Save(PaymentMethod paymentMethod)
        {
            dbConnection.Open();
            using var transaction = CreateTransaction();
            const string sql = @"INSERT INTO PaymentMethod (UserId, Description, DeadLine, CreatedDate) OUTPUT INSERTED.PaymentMethodId
                                    VALUES (@UserId, @Description, @DeadLine, GETDATE());";
            try
            {
                paymentMethod.PaymentMethodId = await (dbConnection.ExecuteScalarAsync<int>(sql, paymentMethod, transaction));
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                var errorMessage = $@"Error guardando un nuevo método de pago";
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public async Task Delete(int paymentMethodId)
        {
            dbConnection.Open();
            using var transaction = CreateTransaction();
            const string sql = @"DELETE FROM PaymentMethod WHERE PaymentMethodId = @paymentMethodId;";
            try
            {
                await (dbConnection.ExecuteAsync(sql, new { paymentMethodId }, transaction));
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                var errorMessage = $@"Error guardando un nuevo método de pago";
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }
}
