# COMP 2001 Report 
## This project is an ASP.NET Core API Micro-Service
### The App
This is a Trail and Wellbeing Application designed to track users walks in and around nature. The scope of this project is to collect store and view user details relevant to their profile (such as a username, email and password).  
This is along with an accompaning report to detail the Legal, Social, Ethical and Professional (LSEP) issues that are accompanied with storing sensitive user data, as well as internet communication of this data. 

### The code
We utilise ASP.NET Core, a C# framework to handle the connections in and out of the database. The other micro-services necessary for the application to run are outside the scope of this project. We use an external authentication API hosted at https://web.socem.plymouth.ac.uk/COMP2001/auth/api/user to determine user roles with the application. These roles give or remove access to different operations within the API, so no sensitive information is viewed or destroyed by those who should not be bale to access it. 

### The database
This database only handles the storage of the Profile element of our page. It consists of alot of tables which have not been fully simulated here, only the User and Archive tables, as they demonstarate a proof on concept over a fully functioning profile and account service.  
