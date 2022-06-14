# FisTracker

Simple application to track worked time and remaining time to work for the rest of month. 

It utilizes Google Cloud Vision API to analyze screenshot of our timesheet (It was not possible to copy just text).

Application is developed in .NET 5 (because our hosting does not support .NET 6 :( ) with Entity Framework Core connecting to MySQL database. 
On Frontend application uses Vue.js with Vuetify as UI library.
