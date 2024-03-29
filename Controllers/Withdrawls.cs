using MyApiBank.Context;
using MyApiBank.Models;
using Microsoft.AspNetCore.Mvc;

// Withdrawls es la simulacion de una transaccion de retiro en la aplicacion bancaria
namespace MyApiBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class Withdrawls : ControllerBase
    {
        private readonly BancaContext _BankContext;

        public Withdrawls(BancaContext context)
        {
            _BankContext = context;
        }


        // POST api/Withdrawls/NewWithdrawl
        // Realiza un retiro en la cuenta del usuario
        // Para realizar el retiro, es necesario:
        // - AccountId: El ID de la cuenta
        // - Amount: La cantidad a retirar
        // - Trantype: El tipo de transaccion (Deposit o Withdrawl)
        // Por ejemplo:
        // {
        //     "AccountId": 1,
        //     "Amount": 100,
        //     "Trantype": "withdrawl"
        // }
        [HttpPost("NewWithdrawl")]
        public async Task<IActionResult> PostWithdrawl([FromBody] Transaction withdrawl)
        {
            try
            {
                // Encuentra la cuenta por AccountID
                var account = await _BankContext.Accounts.FindAsync(withdrawl.AccountId);
                if (account == null)
                {
                    return NotFound();
                }
                // Revisa si la cantidad a retirar es mayor al saldo de la cuenta
                if (account.Balance < withdrawl.Amount)
                {
                    return BadRequest("Fondos insuficientes");
                }

                // Verifica si la cantidad a retirar es mayor a 0
                if (withdrawl.Amount <= 0)
                {
                    return BadRequest("El monto a retirar debe ser mayor a 0");
                }


                // Agrega la transacción a la lista de transacciones 
                // Verifique si el tipo de transacción es "retiro"
                if (withdrawl.Trantype.ToLower() == "withdrawl")
                {
                    // Realiza el retiro de la cuenta
                    account.Balance -= withdrawl.Amount;
                    account.Transaction_list++;
                    account.Withdrawals++;
                }else{
                    return BadRequest("Tipo de transacción no válido, utilice 'withdrawl'");
                }


                // Save the changes to the database
                await _BankContext.SaveChangesAsync();
                return Ok("¡Felicidades! ¡Tu retiro fue exitoso!");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
