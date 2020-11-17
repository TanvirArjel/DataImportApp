# Data Import App
This app is used to import data from csv and xml file and store them into SQL server database.

# Application Architecture

This application has been designed by following **Clean Architecture** and **Domain Driven Design**.

# How to run the app?
1. First make `DataImportApp.Api` project as startup project.
2. Now select `DataImportApp.Infrastructure` project in **Package Manager Console** and run `Update-Database` command to create necessary database tables.
3. Now make both `DataImportApp.Api` and `RazorPageApp` as startup projects.
4. Now run the apps.

# CSV and XML file format for testing
## CSV file content should be as follows:

    "Invoice0000001","1,000.00", "USD", "20/02/2019 12:33:16", "Approved"
    "Invoice0000002","300.00","USD","21/02/2019 02:04:59", "Failed"
    
## XML file content should be as follows:
```
<Transactions>
  <Transaction id="Inv00001">
    <TransactionDate>2019-01-23T13:45:10</TransactionDate>
    <PaymentDetails>
      <Amount>200.00</Amount>
      <CurrencyCode>USD</CurrencyCode>
    </PaymentDetails>
    <Status>Done</Status>
  </Transaction>
  <Transaction id="Inv00002">
    <TransactionDate>2019-01-24T16:09:15</TransactionDate>
    <PaymentDetails>
      <Amount>10000.00</Amount>
      <CurrencyCode>EUR</CurrencyCode>
    </PaymentDetails>
    <Status>Rejected</Status>
  </Transaction>
</Transactions>
```
