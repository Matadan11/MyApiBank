using MyApiBank.Context;
using MyApiBank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;

// Deposit es la simulacion de una transaccion de deposito en la aplicacion bancaria
namespace MyApiBank.Controllers{

    [Route("api/[Controller]")]
    [ApiController]
    public class Deposit : ControllerBase
    {
        private readonly BancaContext _BankContext;

        public Deposit(BancaContext context)
        {
            _BankContext = context;
        }
        // POST api/Deposit/deposit
        // Realiza un deposito en la cuenta del usuario
        // Para realizar el deposito, es necesario:
        // - AccountId: El ID de la cuenta
        // - Amount: La cantidad a depositar
        // - Trantype: El tipo de transaccion (Deposit o Withdrawl)
        // Por ejemplo: 
        // {
        //     "AccountId": 1,
        //     "Amount": 100,
        //     "Trantype": "deposit"
        // }
        [HttpPost("NewDeposit")]
        public async Task<IActionResult> PostDeposit([FromBody] Transaction deposit)
        {
            try
            {
                // Encuentra la cuenta por ID de cuenta
                var account = await _BankContext.Accounts.FindAsync(deposit.AccountId);
                if (account == null)
                {
                    return NotFound();
                }

                // Verifica si la cantidad a depositar es mayor a 0
                if (deposit.Amount <= 0)
                {
                    return BadRequest("La cantidad a depositar debe ser mayor a 0");
                }
            
                // Añadir la transacción a la lista de transacciones
                // Verifica si el tipo de transacción es "depósito"
                if (deposit.Trantype.ToLower() == "deposit")
                {
                    // Realiza el deposito en la cuenta
                    // Actualizar el saldo de la cuenta
                    account.Balance += deposit.Amount;                    // Incrementar el recuento de depósitos en la lista de transacciones de la cuenta
                    account.Transaction_list++;
                    account.Deposits++;
                }

                // Guarde los cambios en la base de datos.
                await _BankContext.SaveChangesAsync();
                return Ok("¡Felicidades! ¡Tu depósito fue exitoso!");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }   
    
        // POST api/Deposit/NewTransfer
        // Realiza una transferencia entre cuentas
        // Para realizar la transferencia, es necesario:
        // - AccountId: El ID de la cuenta de origen
        // - AccountIdDestino: El ID de la cuenta destino
        // - Amount: La cantidad a transferir
        // - Trantype: El tipo de transaccion (transfer)
        // Por ejemplo:
        // {
        //     "AccountId": 1,
        //     "Amount": 100,
        //     "AccountIdDestino": 2,
        //     "Trantype": "transfer"
        // }
        [HttpPost("NewTransfer")]
        public async Task<IActionResult> PostTransfer([FromBody] Transaction transfer)
        {   
            // Encuentra la cuenta de origen por AccountID
            var account = await _BankContext.Accounts.FindAsync(transfer.AccountId);
            if (account == null)
            {
                return NotFound("Cuenta origen no encontrada");
            }

            // Encuentra la cuenta destino por AccountID
            var accountDestino = await _BankContext.Accounts.FindAsync(transfer.AccountId);
            if (accountDestino == null)
            {
                return NotFound("Cuenta destino no encontrada");
            }

            // Realiza la transferencia por medio del Stored Procedure "TransferMoney"
            try
            {
                Console.WriteLine("Monto: " + transfer.Amount);
                // Configura el stored procedure
                var command = _BankContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "EXEC [dbo].[TransferMoney] @AccountId=" + account.AccountId + "@Monto=" + transfer.Amount + "@accountIdDestino=" + accountDestino.AccountId;

                // Abre la conexión
                _BankContext.Database.OpenConnection();

                var reader = command.ExecuteReader();

                reader.Read();

                // Cierra la conexión
                _BankContext.Database.CloseConnection();



                // Guarda los cambios en la base de datos
                await _BankContext.SaveChangesAsync();

                // Verifica si el tipo de transacción es "transfer"
                if (transfer.Trantype.ToLower() == "transfer")
                {
                    // Actualiza un Transaction_list en la cuenta de origen
                    account.Transaction_list++;
                    // Actualiza un Transaction_list en la cuenta destino
                    accountDestino.Transaction_list++;

                    return Ok("Transferencia de cuenta " + account.AccountId + " a cuenta " + accountDestino.AccountId + " exitosa");
                }
                else
                {
                    return BadRequest("Tipo de transacción inválido, por favor use 'transfer'");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    
    }

}
