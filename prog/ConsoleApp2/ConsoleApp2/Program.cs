using System;
using MySql.Data.MySqlClient;

namespace ConsoleApp2
{
    class Program
    {
        static void PrintMenu()
        {
            Console.WriteLine("+++++++++++++++++++++++++++");
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Создать нового клиента");
            Console.WriteLine("2. Создать новый счёт с привязкой к клиенту");
            Console.WriteLine("3. Создать новый кредит с привязкой к клиенту");
            Console.WriteLine("4. Показать остатки по счетам");
            Console.WriteLine("5. Показать остатки по кредитам");
            Console.WriteLine("6. Показать информацию о клиентах");
            Console.WriteLine("0. Выход");
            Console.WriteLine("+++++++++++++++++++++++++++");
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Введите пункт меню - ");
            int number;
            do {
                PrintMenu();
                number = Convert.ToInt32(Console.ReadLine());
                //подключение к БД
                var conn_str = "Server=mysql60.hostland.ru;Database=host1323541_itstep37;Uid=host1323541_itstep;Pwd=269f43dc;";
                var connection = new MySqlConnection(conn_str);
                connection.Open();
                switch (number)
                {
                    case 1://добавление пользователя
                    {
                        Console.Clear();
                        Console.WriteLine("Введите ФИО нового пользователя:");
                        var newUser = Console.ReadLine();
                        Console.WriteLine("Введите дату регистрации нового пользователя:");
                        var newDayRegistration = Console.ReadLine();
                        var sql = $"INSERT INTO host1323541_itstep37.table_client (name, date_registration) VALUES ('{newUser}', '{newDayRegistration}');";
                        var command = new MySqlCommand
                        {
                            Connection = connection, CommandText = sql
                        };
                        var result = command.ExecuteNonQuery();
                        if (result == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Пользователь НЕ добавлен");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("Пользователь добавлен");
                            Console.ResetColor();
                        }
                        connection.Close();
                    }
                        break;
                    case 2: //создать новый счет
                    {
                        Console.Clear();
                        Console.WriteLine("Укажите ID пользователя:");
                        var user_id = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Укажите дату открытия счета:");
                        var date_open = Console.ReadLine();
                        Console.WriteLine("Укажите сумму счета:");
                        var invoice_amount = Convert.ToInt32(Console.ReadLine());
                        var sql = $"INSERT INTO host1323541_itstep37.table_contribution (invoice_amount, client_id, date_open) VALUES ({invoice_amount}, {user_id}, '{date_open}');";
                        var command = new MySqlCommand
                        {
                            Connection = connection, CommandText = sql
                        };
                        var result = command.ExecuteNonQuery();
                        if (result == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Счет НЕ добавлен");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine($"Счет на сумму {invoice_amount} рублей добавлен");
                            Console.ResetColor();
                        }
                        connection.Close();
                    }
                        break;
                    case 3: //добавить кредит
                    {
                        Console.Clear();
                        Console.WriteLine("Укажите ID пользователя:");
                        var user_id = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Укажите дату выдачи кредита:");
                        var date_open = Console.ReadLine();
                        Console.WriteLine("Укажите сумму кредита:");
                        var credit_amount = Convert.ToInt32(Console.ReadLine());
                        var sql = $"INSERT INTO host1323541_itstep37.table_credit (credit_amount, client_id, date_open) VALUES ({credit_amount}, {user_id}, '{date_open}');";
                        var command = new MySqlCommand
                        {
                            Connection = connection, CommandText = sql
                        };
                        var result = command.ExecuteNonQuery();
                        if (result == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Кредит НЕ выдан");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine($"Кредит на сумму {credit_amount} рублей выдан");
                            Console.ResetColor();
                        }
                        connection.Close();
                        
                    }
                        break;
                    case 4://остатки по счетам
                    {
                        Console.Clear();
                        var sql = $"SELECT name, SUM(invoice_amount) AS sum FROM table_client, table_contribution WHERE table_contribution.client_id=table_client.id GROUP BY table_client.id;";
                        var command = new MySqlCommand
                        {
                            Connection = connection, CommandText = sql
                        };
                        var result = command.ExecuteReader();
                        Console.WriteLine("Остатки по счетам:");
                        while (result.Read())
                        {
                            var tempName = result.GetString("name");
                            var tempSum = result.GetString("sum");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"{tempName} - {tempSum}");
                        }
                        connection.Close();
                    }
                        break;
                    case 5: //остатки по кредитам
                    {
                        Console.Clear();
                        var sql = $"SELECT name, SUM(credit_amount) AS sum FROM table_client, table_credit WHERE table_credit.client_id=table_client.id GROUP BY table_client.id;";
                        var command = new MySqlCommand
                        {
                            Connection = connection, CommandText = sql
                        };
                        var result = command.ExecuteReader();
                        Console.WriteLine("Остатки по кредитам:");
                        while (result.Read())
                        {
                            var tempName = result.GetString("name");
                            var tempSum = result.GetString("sum");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"{tempName} - {tempSum}");
                        }
                        connection.Close();
                    }
                        break;
                    case 6: //информация о клиентах
                    {
                        Console.Clear();
                        var sql = $"SELECT * FROM table_client;";
                        var command = new MySqlCommand
                        {
                            Connection = connection, CommandText = sql
                        };
                        var result = command.ExecuteReader();
            
                        while (result.Read())
                        {
                            var tempName = result.GetString("name");
                            var tempDate = result.GetDateTime("date_registration");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"{tempName} - {tempDate:G}");
                        }
                        connection.Close();
                    }
                        break;
                }
            } while (number != 0);
        }
    }
}

