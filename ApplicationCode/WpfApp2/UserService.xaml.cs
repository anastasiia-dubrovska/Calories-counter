﻿// <copyright file="UserService.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CaloriesCounter
{
    using Npgsql;
    using System;
    using System.Net;
    using System.Net.Mail;

    public class UserService
    {
        private readonly string connectionString = "Host=localhost;Database=Project111;Username=postgres;Password=your_new_password";

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            using (var connection = new NpgsqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE username = @username AND password = @oldPassword", connection);
                _ = command.Parameters.AddWithValue("@username", username);
                _ = command.Parameters.AddWithValue("@oldPassword", oldPassword);

                var count = Convert.ToInt32(command.ExecuteScalar());

                if (count == 0)
                {
                    return false;
                }

                command = new NpgsqlCommand("UPDATE users SET password = @newPassword WHERE username = @username", connection);
                _ = command.Parameters.AddWithValue("@newPassword", newPassword);
                _ = command.Parameters.AddWithValue("@username", username);

                _ = command.ExecuteNonQuery();
                return true;
            }
        }

        public void SendFeedback(string userEmail, string feedback)
        {
            try
            {
                var fromAddress = new MailAddress("caloriescounterproject@gmail.com", "CaloriesCounter");
                var toAddress = new MailAddress(userEmail);
                const string fromPassword = "qwertyuiop1237@";
                const string subject = "Feedback from Calories Counter";
                string body = feedback;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
