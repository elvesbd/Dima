-- Get Expenses By Category (V1)
CREATE OR ALTER VIEW [vwGetExpensesByCategory] AS
    SELECT
        [Transaction].[UserId],
        [Category].[Title] AS [Category],
        YEAR([Transaction].[PaidOrReceived]) AS [Year],
        SUM([Transaction].[Amount]) AS [Expenses]
    FROM [Transaction]
        INNER JOIN [Category] ON [Transaction].[CategoryId] = [Category].[Id]
    WHERE
        [Transaction].[PaidOrReceived]
        >= DATEADD(MONTH, -11, CAST(GETDATE() AS DATE)) AND
        [Transaction].[PaidOrReceived]
        < DATEADD(MONTH, -1, CAST(GETDATE() AS DATE)) AND
        [Transaction].[Type] = 2
    GROUP BY
        [Transaction].[UserId],
        [Category].[Title],
        YEAR([Transaction].[PaidOrReceived])