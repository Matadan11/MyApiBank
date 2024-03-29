CREATE PROCEDURE TransferMoney
    @AccountId INT,
    @Monto DECIMAL(18,2),
    @AccountIdDestino INT
AS
BEGIN
    -- Validar que el monto a transferir sea mayor a 0
    IF @Monto <= 0
    BEGIN
        RAISERROR('El monto a transferir debe ser mayor a 0', 16, 1);
        RETURN;
    END

    -- Validar que la cuenta de origen tenga suficiente saldo
    IF (SELECT Balance FROM Accounts WHERE AccountId = @AccountId) < @Monto
    BEGIN
        RAISERROR('La cuenta de origen no tiene suficiente saldo', 16, 1);
        RETURN;
    END

    -- Validar que la cuenta de destino exista
    IF NOT EXISTS (SELECT 1 FROM Accounts WHERE AccountId = @AccountIdDestino)
    BEGIN
        RAISERROR('La cuenta de destino no existe', 16, 1);
        RETURN;
    END

    --Validar que la cuenta de origen y destino no sean la misma
    IF @AccountId = @AccountIdDestino
    BEGIN
        RAISERROR('La cuenta de origen y destino no pueden ser la misma', 16, 1);
        RETURN;
    END

    -- Restar el monto de la cuenta de origen y sumar withdraw +1
    UPDATE Accounts
    SET Balance = Balance - @Monto,
        Withdraws = Withdraws + 1
    WHERE AccountId = @AccountId;

    -- Sumar el monto a la cuenta de destino y sumar deposit +1
    UPDATE Accounts
    SET Balance = Balance + @Monto,
        Deposits = Deposits + 1
    WHERE AccountId = @AccountIdDestino;

    -- Actualiza transaction_List +1 para ambas cuentas
    UPDATE Accounts
    SET Transaction_List = Transaction_List + 1
    WHERE AccountId = @AccountId;

    UPDATE Accounts
    SET Transaction_List = Transaction_List + 1
    WHERE AccountId = @AccountIdDestino ;
    
    -- Crear un registro en la tabla de transacciones
    INSERT INTO Transactions (AccountId, Amount, TransactionDate, TranType)
    VALUES (@AccountId, @Monto, GETDATE(), 'Transfer');
END