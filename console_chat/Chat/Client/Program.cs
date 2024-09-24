using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        while (true)
        {
            // 1. Устанавливаем конечную точку для подключения (IP и порт сервера)
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 11000);

            // 2. Создаем сокет для соединения с сервером
            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // 3. Подключаемся к серверу
                sender.Connect(endPoint);
                Console.WriteLine("Введите сообщение (или 'exit' для выхода):");

                // 4. Читаем сообщение от пользователя
                string message = Console.ReadLine();

                // Проверяем, хочет ли пользователь выйти
                if (message == "exit")
                {
                    break;  // Прерываем цикл и завершаем программу
                }

                // 5. Преобразуем сообщение в массив байт
                byte[] msg = Encoding.UTF8.GetBytes(message);

                // 6. Отправляем сообщение на сервер
                sender.Send(msg);

                // 7. Получаем ответ от сервера
                byte[] buffer = new byte[1024];
                int bytesRec = sender.Receive(buffer);

                // 8. Преобразуем полученные байты в строку
                Console.WriteLine("Ответ от сервера: " + Encoding.UTF8.GetString(buffer, 0, bytesRec));
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
            }
            finally
            {
                // 9. Закрываем сокет
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
        }
    }
}