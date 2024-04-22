using FlightManagementAPI.Infrastructure.Exceptions;
using FlightManagementData.Data;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementAPI.Helpers.DatabaseOperations
{
    public class DatabaseOperationHandler
    {
        public static async Task HandleDatabaseOperation(Func<Task> databaseOperation, string operationName)
        {
            try
            {
                await databaseOperation.Invoke();
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException($"Błąd podczas operacji {operationName} w bazie danych.", ex);
            }
        }
        public static async Task<IActionResult> TryToSaveChangesAsync(FlightManagementContext context)
        {
            try
            {
                await DatabaseOperationHandler.HandleDatabaseOperation(
                    async () => await context.SaveChangesAsync(),
                    "zapisu danych w bazie");
                return new OkResult();
            }
            catch (DatabaseOperationException ex)
            {
                throw new BadRequestException($"Błąd operacji w bazie danych: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Błąd operacji w bazie danych: {ex.Message}");
            }
        }
    }
}
