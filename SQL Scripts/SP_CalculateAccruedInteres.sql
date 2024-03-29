CREATE PROCEDURE CalculateAccruedInterest
    @accountId INT,
    @amount DECIMAL(10, 2)
AS
BEGIN
    -- Declare variables
    DECLARE @accruedInterest DECIMAL(10, 2)
    DECLARE @balance DECIMAL(10, 2)
    DECLARE @interestRate DECIMAL(10, 2)

    -- Establecer la tasa de interés predeterminada
    Set @interestRate = 0.05

    -- Obtenga el saldo de la cuenta
    SELECT @balance = Balance
    FROM Accounts
    WHERE account_id = @accountId

    -- Verifique si no tiene saldo la cuenta
    IF @balance IS NULL
    BEGIN
        RAISERROR('La cuenta no tiene saldo', 16, 1)
        RETURN
    END

    -- Calcule el interés acumulado
    SET @accruedInterest = @balance * @interestRate

    -- El interés acumulado no puede ser mayor que el monto
    IF @accruedInterest > @amount
    BEGIN
        RAISERROR('El interés acumulado no puede ser mayor que el monto', 16, 1)
        RETURN
    END

    -- muestra el interés acumulado en la cuenta
    SELECT @accruedInterest AS AccruedInterest

    RETURN @accruedInterest

END