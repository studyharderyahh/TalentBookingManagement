Project Name: Talent Booking Management System
Version: 1.0.0

Authors: Elisa Wu, Yerin Kim, Yvette Wang

Description:
This project is to develop a Talent Booking Management System which will have below key components:

    Talent Management: Tracks individual talents' details, availability, skills, and rates.
    Staff Management: Manages agency personnel information and access permissions.
    Booking Management: Handles client requests, booking details, payment tracking, and invoicing.
	
Contributors:
*** Talent Management by Yerin Kim *** 

Functionality: Add Talent, Inactivate Talent, Activate Talent, Search Talent, View Talents

Files:

│   LoginWindow.xaml
│   LoginWindow.xaml.cs
│   MainWindow.xaml
│   MainWindow.xaml.cs

├───TalentManagement
│       ActivateTalentWindow.xaml
│       ActivateTalentWindow.xaml.cs
│       AddTalentWindow.xaml
│       AddTalentWindow.xaml.cs
│       InActivateTalentWindow.xaml
│       InActivateTalentWindow.xaml.cs
│       ReadTalentList.xaml
│       ReadTalentList.xaml.cs
│       SearchingTalentWindow.xaml
│       SearchingTalentWindow.xaml.cs


*** Staff and Client Management by Yvette Wang ***
Functionality: Add Staff, View Staff, Update Staff, View Staff Permissions, Delete Staff, Add Client, View Client, Update Client
│
├───ViewModels
│       AddNewClientViewModel.cs
│       AddNewStaffViewModel.cs
│       ClientDetailsViewModel.cs
│       DeleteStaffViewModel.cs
│       StaffDetailsViewModel.cs
│       StaffPermissionViewModel.cs
│       UpdateClientViewModel.cs
│       UpdateStaffViewModel.cs
│
└───Views
    │
    └───Staff&ClientManagement
            AddNewClient.xaml
            AddNewClient.xaml.cs
            AddNewStaff.xaml
            AddNewStaff.xaml.cs
            DeleteStaff.xaml
            DeleteStaff.xaml.cs
            UpdateClient.xaml
            UpdateClient.xaml.cs
            UpdateSatff.xaml
            UpdateSatff.xaml.cs
            ViewClientDetails.xaml
            ViewClientDetails.xaml.cs
            ViewStaffDetails.xaml
            ViewStaffDetails.xaml.cs
            ViewStaffPermission.xaml
            ViewStaffPermission.xaml.cs


*** Booking Management by Elisa Wu ***

Functionality: Add Booking, View Bookings, Update Booking, Cancel Booking, Create Campaign, View Campaign, Search Talent, Create Client, View Client

Files:
------
├───Converters
│       DecimalConverter.cs
│
├───FileHelper
│       Logger.cs
│
├───Models
│       Booking.cs
│       Campaign.cs
│       Client.cs
│       Person.cs
│       Talent.cs
│
├───ViewModels
│       BaseViewModel.cs
│       CancelBookingViewModel.cs
│       CreateCampaignViewModel.cs
│       CreateClientViewModel.cs
│       MakeBookingViewModel.cs
│       RelayCommand.cs
│       SelectTalentViewModel.cs
│       UpdateBookingViewModel.cs
│       ViewBookingsViewModel.cs
│
└───Views
    │   CancelBookingWindow.xaml
    │   CancelBookingWindow.xaml.cs
    │   CreateCampaignWindow.xaml
    │   CreateCampaignWindow.xaml.cs
    │   CreateClientWindow.xaml
    │   CreateClientWindow.xaml.cs
    │   MakeBookingWindow.xaml
    │   MakeBookingWindow.xaml.cs
    │   SelectTalentWindow.xaml
    │   SelectTalentWindow.xaml.cs
    │   UpdateBookingWindow.xaml
    │   UpdateBookingWindow.xaml.cs
    │   ViewBookingsWindow.xaml
    │   ViewBookingsWindow.xaml.cs


Out of Scope: Payments, Login, Invoice and Notifications were excluded due to time constraints and it was agreed by Lecturer David McCurdy.
These will be part of future improvements. Watch this space :)

A huge thanks to David McCurdy for his guidance, support and help through this project.