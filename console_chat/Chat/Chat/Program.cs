using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class MultiThreadedServer
{
    static void Main()
    {
        // 1. Устанавливаем конечную точку для сервера (IP и порт)
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 11000);
        
        // 2. Создаем сокет для прослушивания подключений
        Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        // 3. Привязываем сокет к конечной точке
        listener.Bind(endPoint);
        
        // 4. Начинаем прослушивание с очередью на 10 клиентов
        listener.Listen(10);
        
        Console.WriteLine("Сервер запущен. Ожидаем подключений...");

        while (true)
        {
            // 5. Принимаем подключение клиента (блокирующий вызов)
            Socket clientSocket = listener.Accept();
            
            // 6. Для каждого клиента запускаем новый поток для его обслуживания
            Thread clientThread = new Thread(() => HandleClient(clientSocket));
            clientThread.Start();
            Console.WriteLine("Loop is over!!!!!!!!!!!!!!!!!!!!!!!");
        }
    }

    // 7. Метод для обработки клиента в отдельном потоке
    static void HandleClient(Socket clientSocket)
    {
        // Буфер для приема данных
        byte[] buffer = new byte[1024];

        try
        {
            // Прием данных от клиента
            int bytesRec = clientSocket.Receive(buffer);
            string data = Encoding.UTF8.GetString(buffer, 0, bytesRec);
            Console.WriteLine("Получено от клиента: " + data);

            // Подготовка ответа
            byte[] msg = Encoding.UTF8.GetBytes("Принято: " + data);

            // Отправка данных клиенту
            clientSocket.Send(msg);
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка: " + e.Message);
        }
        finally
        {
            // Закрытие соединения
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }
}
