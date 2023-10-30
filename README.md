# Hospitalwebapp
Web portal for a GP/hospital to offer as a service online. Both patients and doctors can register an account through the website. Once their account is manually verified by an administrator ,doctors will be manually assigned to the relevant patients. Doctors and patients then can exchange secure texts and communicate remotely through a chat system.

Specifically functionality exists to allow patients to:

•	Create an account using an email and password

•	Communicate with medical professionals about general issues

Additionally functionality to allow doctors / medical staff to:

•	Create an account using an email and password

•	Respond to patients’ general queries

Finally functionality to allow an admin to:

•	Approve an account as a genuine doctor/member of hospital staff

•	Approve account as genuine patient

•	Assign Doctors to relevant patient’s accounts


This project makes use of Svelte C# and Typescript to create a Web server.
Firebase acts as the authentication server.
 MongoDB is used as a database. 

To see showcase of functionality open Functionality.docx

![image](https://github.com/GeorgeRobinson365/Hospital-Chat-App/assets/110357060/3a1221b2-cc2f-41a4-a267-b21f027c3346)


To initalise: 
1. Launch backend.API.sln (filepath: backend/src/Backend.API/Backend.API.sln) through visual studio. This creates backend web server.
2. Open Frontend folder in visual studio code and open terminal. 
3. In terminal input "npm install", wait for installation of required packages.
4. In terminal input "npm run dev" and navigate to localhost website was created on (default is http://localhost:3000/ )
5. Localhost will show Account creation / login screen. From here you can login using login information below:

Admin panel login:
Email: Admin@hospital.net
Password: Admin123!

John Smith (Doctor)
Email: JohnSmith@gmail.com	
Password: JohnSmith@gmail.com 

Joel Smarts (Patient)
Email: JoelSmarts@gmail.com
Pass: JoelSmarts@gmail.com 
