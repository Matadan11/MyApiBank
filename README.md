# MyApiBank

  Esta Api permitirá realizar las siguientes acciones:
  
- Obtener información de la cuenta bancaria, incluido el saldo actual, la lista de transacciones y los intereses devengados.
- Agregar depósitos a una cuenta.
- Crear retiros desde una cuenta.
- Transferir dinero entre cuentas.

# Configuraciones Principales:
Instalar los siguientes paquetes:
![image](https://github.com/Matadan11/MyApiBank/assets/102993860/7dcaf18b-f330-47c9-aaf7-55471a936024)

# Configuracion de BD
La Base de datos fue creada por medio del Entity.Framework.Core

Los comandos utilizados para realizar esto automaticamente [recomendado] son:

En Visual Studio > Herramientas

![image](https://github.com/Matadan11/MyApiBank/assets/102993860/fb5e5503-97f4-4af1-b0ca-87655b1f2e2a)

PM> Add-Migration Initial

PM> Update-Databaase

En caso de algun error, revisar la cadena de conexion por medio de **appsettings.json**

Al ejecutar estos comandos la BD debe estar creada con las tablas necesarias...

En la carpeta de "SQL Script" esta la creacion de BD y los diferentes Stored Procedures

# Uso del Api
## EndPoints
![image](https://github.com/Matadan11/MyApiBank/assets/102993860/9f5fbd23-c1cb-41af-9216-cfffa19cff09)


# Indicaciones para realizar pruebas:
## Agregar y consultar cuentas

### api/myInfoAccount/{AccountID}

Deberá primero agregar una cuenta por medio del endpoint: 

**api/myInfoAccount/NewAccount**

Puede usar el siguiente Json para crear una nueva cuenta:

{
  "accountNumber": "String"
}

Para obtener la informacion de la cuenta debera enviar el **"accountId"**

Respuesta:
{
  "accountId": 0,
  "accountNumber": "string",
  "balance": 0,
  "transaction_list": 0,
  "deposits": 0,
  "withdrawals": 0,
  "accrued_interest": 0
}

## Depósitos
### api/Deposit/NewDeposit

Para realizar el depósito necesita enviar:

Ejemplo:

{
  "accountId": 1,
  "amount": 10,
  "trantype": "deposit"
}

### api/Deposit/NewTransfer

La transferencia se comportará como un deposito nuevo para la cuenta destino y como un retiro para la cuenta de origen

### **OJO**

Es requerido enviar la transferencia de la siguiente manera:

{
    "AccountId": 1,
    "Amount": 10,
    "AccountIdDestino": 2,
    "Trantype": "transfer"
}

## Retiros

Para 








