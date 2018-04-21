Azure Functions Intro for VS Code
==================================
https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local


Steps to get lab setup
----------------------
1.  Create a Resource Group called "AzureWorkshop" in Azure Portal (portal.azure.com)
2.  Create a Storage account called "azureworkshopdme" (use your own initials)
3.  Create a queue called "alerts"
4.  Open cmd prompt and run "npm install -g azure-functions-core-tools@core"
5.  Run "func init AzureFunctions"
6.  Run "func new"
7.  Select "C#"
8.  Select "Queue Trigger"
9.  Give the name of "ProcessAlerts"
10. Set your storage key (AzureWebJobsStorage) in local.settings.json
11. Change queueName to "alerts" in ProcessAlerts\function.json
12. Run "func start ProcessAlerts"
