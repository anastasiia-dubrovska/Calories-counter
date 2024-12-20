﻿// <copyright file="RegisterWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CaloriesCounter
{
    using Npgsql;
    using System;
    using System.Windows;

    public partial class RegisterWindow : Window
    {
        private readonly MainWindow mainWindow;
        private readonly string connectionString = "Host=localhost;Database=Project111;Username=postgres;Password=your_new_password";

        public RegisterWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainWindow.Show();
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                _ = MessageBox.Show("Будь ласка, заповніть всі поля.");
                return;
            }

            if (password != confirmPassword)
            {
                _ = MessageBox.Show("Паролі не співпадають.");
                return;
            }

            using (var connection = new NpgsqlConnection(this.connectionString))
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(1) FROM users WHERE email = @Email";
                using (var checkCommand = new NpgsqlCommand(checkQuery, connection))
                {
                    _ = checkCommand.Parameters.AddWithValue("@Email", email);
                    int userExists = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (userExists > 0)
                    {
                        _ = MessageBox.Show("Користувач з такою електронною поштою вже існує.");
                        return;
                    }
                }

                string insertQuery = "INSERT INTO users (email, password, name) VALUES (@Email, @Password, @Name)";
                using (var insertCommand = new NpgsqlCommand(insertQuery, connection))
                {
                    _ = insertCommand.Parameters.AddWithValue("@Email", email);
                    _ = insertCommand.Parameters.AddWithValue("@Password", password);
                    _ = insertCommand.Parameters.AddWithValue("@Name", email);
                    _ = insertCommand.ExecuteNonQuery();
                }
            }

            _ = MessageBox.Show("Реєстрація успішна!");

            var mainUserWindow = new MainProgramWindow(1, email);
            mainUserWindow.Show();
            this.Close();
        }

        private void GoToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow(this.mainWindow);
            loginWindow.Show();
            this.Close();
        }

        private void EmailBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EmailBox.Text == "Введіть електронну пошту")
            {
                EmailBox.Text = string.Empty;
                EmailBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void EmailBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailBox.Text))
            {
                EmailBox.Text = "Введіть електронну пошту";
                EmailBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == "Введіть пароль")
            {
                PasswordBox.Clear();
                PasswordBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                PasswordBox.Password = "Введіть пароль";
                PasswordBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void ConfirmPasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ConfirmPasswordBox.Password == "Підтвердіть пароль")
            {
                ConfirmPasswordBox.Clear();
                ConfirmPasswordBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void ConfirmPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
            {
                ConfirmPasswordBox.Password = "Підтвердіть пароль";
                ConfirmPasswordBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
    }
}
