## 🏥 Medical Appointment Booking System

A **.NET Core** based medical appointment booking system that allows patients to find doctors, book appointments, and manage their bookings efficiently. Inspired by **Vezeeta**, this system is designed to provide a seamless experience for both patients and doctors.

---

## ✨ Features  

✅ **User Authentication**: Secure login and registration using **JWT & Identity Framework**.  
✅ **Role-based Access**: Different roles for **Patients** and **Doctors**.  
✅ **Appointment Booking**: Patients can book, cancel, or reschedule appointments.  
✅ **Doctor Availability**: Doctors can set their available time slots.  
✅ **Search & Filtering**: Patients can search for doctors by **specialty, location, and availability**.  
✅ **Email Notifications**: Confirmation emails for appointments.  
✅ **Admin Panel (Future Enhancement)**: Manage users, doctors, and appointments.  

---

## 🛠️ Tech Stack  

🔹 **Backend**: `.NET Core 8`, `ASP.NET Core Web API`, `C#`  
🔹 **Database**: `SQL Server`, `Entity Framework Core`, `LINQ`  
🔹 **Authentication**: `JWT`, `Identity Framework`  
🔹 **API Testing**: `Postman`  
🔹 **Version Control**: `Git`, `GitHub`  
🔹 **Frontend (Planned)**: `Flutter`  

---

## 🚀 Getting Started  

### 1⃣ Clone the repository  
```bash
git clone https://github.com/yourusername/Medical-Appointment-System.git
cd Medical-Appointment-System
```

### 2⃣ Set up the database  
- Update the connection string in `appsettings.json`.  
- Run migrations:  
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 3⃣ Run the application  
```bash
dotnet run
```
- The API will be available at: `https://localhost:5001/api`

---

## 📌 API Endpoints  

### 🔹 Authentication  
- `POST /api/auth/register` → Register a new user  
- `POST /api/auth/login` → Authenticate and get a JWT  

### 🔹 Appointments  
- `POST /api/appointments/book` → Book a new appointment  
- `GET /api/appointments/upcoming` → View upcoming appointments  
- `DELETE /api/appointments/cancel/{id}` → Cancel an appointment  

### 🔹 Doctors  
- `GET /api/doctors` → List all doctors  
- `GET /api/doctors/{id}` → Get doctor details  

--
🌟 **Contributions are welcome!** Fork the repo, create a new branch, and submit a PR. 🚀  


 
