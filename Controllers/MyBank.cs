using MyApiBank.Context;
using MyApiBank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;


// "MyBank" es una simulacion del app bancaria que permite al usuario ver su cuenta.
namespace MyApiBank.Controllers
{

    [Route("api/[Controller]")]
    [ApiController]
    public class myInfoAccount : ControllerBase
    {
        private readonly BancaContext _BankContext;

        public myInfoAccount(BancaContext context)
        {
            _BankContext = context;
        }

        // GET api/myInfoAccount
        // Obtiene la informacion de todas las  cuentas en BD
        [HttpGet]
        public ActionResult<Account> Get()
        {
            return Ok(_BankContext.Accounts);
        }

        // GET api/myInfoAccount/AccountID
        // Obtiene la informacion de la cuenta del usuario
        // AccountID: El ID de la cuenta
        [HttpGet("{AccountID}")]
        public ActionResult<Account> Get(int AccountID)
        {
            var cuenta = _BankContext.Accounts.FirstOrDefault(x => x.AccountId == AccountID);
            if (cuenta == null)
            {
                return NotFound();
            }

            //obtener el Balance de la cuenta del usuario
            var command = _BankContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = "SELECT Balance FROM Accounts WHERE AccountId = @accountId";
            
            //Configuracion del parametro
            command.Parameters.Add(new SqlParameter("@accountId", AccountID));
            
            _BankContext.Database.OpenConnection();
            
            var reader = command.ExecuteReader();
            
            reader.Read();
 
            cuenta.Balance = reader.GetDecimal(0);

            _BankContext.Database.CloseConnection();

            try
            {   
                //Obtener el accrued_interest de la cuenta del usuario
                command = _BankContext.Database.GetDbConnection().CreateCommand();
                //Configura el stored procedure
                // Se ejecuta a mano por problemas con la libreria que debe ejecutar el stored procedure
                command.CommandText = "EXEC	[dbo].[CalculateAccruedInterest] @accountId=" + AccountID + ", @amount=" + cuenta.Balance;
                
                _BankContext.Database.OpenConnection();
                
                //Ejecutar el stored procedure
                var reader2 = command.ExecuteReader();

                reader2.Read();

                Console.WriteLine("reader: " + reader2);
                var AccruedInterest = reader2.GetDecimal(0);

                //Toma el dato y lo actualiza en la cuenta del usuario
                cuenta.accrued_interest = AccruedInterest; 

                //actualiza en la base de datos
                _BankContext.SaveChanges();
                
                // Cierra la conexion
                _BankContext.Database.CloseConnection();

                return Ok(cuenta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }   
        }


        // POST api/myInfoAccount/NewAccount
        // Crea una nueva cuenta
        // Al crear la cuenta los valores de Balance, Transaction_list, Deposits, Withdrawals y accrued_interest se inicializan en 0
        [HttpPost("NewAccount")]
        public ActionResult<Account> CreateAccount(Account account)
        {
            _BankContext.Accounts.Add(account);
            _BankContext.SaveChanges();
            return CreatedAtAction(nameof(Get), new { AccountID = account.AccountId }, account);
        }

        private string ExecuteStoredProc(int accountID, decimal accruedInterest)
        {
            try
            {
                // Configura el stored procedure
                var bancaContext = new BancaContext();
                var command = bancaContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "CalculateAccruedInterest";
                command.CommandType = CommandType.StoredProcedure;

                // Configura los parámetros si es necesario
                command.Parameters.Add(new SqlParameter("@accountId", accountID));
                command.Parameters.Add(new SqlParameter("@amount", accruedInterest));

                // Ejecuta el stored procedure
                bancaContext.Database.OpenConnection();
                command.ExecuteNonQuery();
                bancaContext.Database.CloseConnection();

                // Return a success response
                return "Success";
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return ($"Error al ejecutar el stored procedure: {ex.Message}");
            }
        }


    }
}




