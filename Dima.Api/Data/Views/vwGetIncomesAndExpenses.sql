CREATE OR ALTER VIEW [vwGetIncomesAndExpenses] AS
    SELECT
        [Transaction].[UserId],
        MONTH([Transaction].[PaidOrReceived]) AS [Month],
        YEAR([Transaction].[PaidOrReceived]) AS [Year],
        SUM(CASE WHEN [Transaction].[Type] = 1 THEN[Transaction].[Amount] ELSE 0 END) AS [Incomes],
        SUM(CASE WHEN [Transaction].[Type] = 2 THEN[Transaction].[Amount] ELSE 0 END) AS [Expenses]
    FROM [Transaction]
    WHERE
        [Transaction].[PaidOrReceived]
        >= DATEADD(MONTH, -11, CAST(GETDATE() AS DATE)) AND
        [Transaction].[PaidOrReceived]
        < DATEADD(MONTH, -1, CAST(GETDATE() AS DATE))
    GROUP BY
        [Transaction].[UserId],
        MONTH([Transaction].[PaidOrReceived]),
        YEAR([Transaction].[PaidOrReceived]);
