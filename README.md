# COMP 2001 Report 
## This project is an ASP.NET Core API Micro-Service
### The App
The code is hosted on a web server accessable here:
https://web.socem.plymouth.ac.uk/COMP2001/adennett/swagger/index.html

This is a Trail and Wellbeing Application designed to track users walks in and around nature. The scope of this project is to collect store and view user details relevant to their profile (such as a username, email and password).  
This is along with an accompaning report to detail the Legal, Social, Ethical and Professional (LSEP) issues that are accompanied with storing sensitive user data, as well as internet communication of this data. 

### Using the Service
The application will deny you access to any requests apart from Login, or creating an account (POST). This is so you can be registered before acccessing any sensitive info. Once you have logged into your account (newly created or not) you will be checked with out with the authentification API, and your role within the service will be determined. If you are a standard user, you can view all other user's usernames as well as edit your own profile. If you an admin you will be able to archive and unarchive users. Only a user who owns an account can edit their own details. 

### The code
We utilise ASP.NET Core, a C# framework to handle the connections in and out of the database. The other micro-services necessary for the application to run are outside the scope of this project. We use an external authentication API hosted at https://web.socem.plymouth.ac.uk/COMP2001/auth/api/user to determine user roles with the application. These roles give or remove access to different operations within the API, so no sensitive information is viewed or destroyed by those who should not be bale to access it. 

### The database
This database only handles the storage of the Profile element of our page. It consists of alot of tables which have not been fully simulated here, only the User and Archive tables, as they demonstarate a proof on concept over a fully functioning profile and account service.  
