using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace DataCollector.Server
{
    public class HttpServer
    {
        HttpListener server;
        bool flag = true;

        public void StartServer(string prefix)
        {
            server = new HttpListener();
            // текущая ос не поддерживается
            if (!HttpListener.IsSupported) return;

            //добавление префикса (testPage/)
            //обязательно в конце должна быть косая черта
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentException("prefix");
            server.Prefixes.Add(prefix);
            //запускаем север
            server.Start();


            //сервер запущен? Тогда слушаем входящие соединения
            while (server.IsListening)
            {
                MessageBox.Show("Сервер в работе!", "CollectorServer");

                //ожидаем входящие запросы
                HttpListenerContext context = server.GetContext();

                //получаем входящий запрос
                HttpListenerRequest request = context.Request;

                //обрабатываем POST запрос
                //если запрос получен методом POST (это например пришли данные c формы от Http сервера)
                if (request.HttpMethod == "POST")
                {
                    //обработать (показать), что пришло от клиента
                    ShowRequestData(request);
                    //завершаем работу сервера
                    if (!flag) return;
                }

                //формируем ответ сервера:

                //динамически создаём фасадную приветственную страницу  отправляемую клиенту
                string responseString = @"<!DOCTYPE HTML>
                     <html><head><style>
                               body {
                               background: #777; /* Цвет фона */
                               color: #fc0; /* Цвет текста */
                               }
                     </style></head><body>
                     <form method=""post"" action=""testPage"">
                     <p><b> Test Page 'CollectorServer' </b><br>
                     <p><b>  </b><br>
                     <p><b> Enter text: </b><br>
                     <input type=""text"" name=""clientSend"" size=""40""></p>
                     <p><input type=""submit"" value=""send""></p>
                     </form></body></html>";

                //отправка данных клиенту
                HttpListenerResponse response = context.Response;
                response.ContentType = "text/html; charset=UTF-8";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                using (Stream output = response.OutputStream)
                {
                    output.Write(buffer, 0, buffer.Length);
                }
            }
        }


        //обработчик запросов полученных сервером
        private void ShowRequestData(HttpListenerRequest request)
        {
            //есть данные от клиента?
            if (!request.HasEntityBody) return;

            //смотрим, что пришло
            using (Stream body = request.InputStream)
            {
                MessageBox.Show("получен запрос от клиента", "CollectorServer");

                using (StreamReader reader = new StreamReader(body))
                {
                    string text = reader.ReadToEnd();
                    //если необоставляем только данные сообщения
                    text = text.Remove(0, 0);

                    //преобразуем 
                    text = System.Web.HttpUtility.UrlDecode(text, Encoding.UTF8);

                    //выводим имя
                    MessageBox.Show("Содержание: " + text, "CollectorServer");
                    flag = true;

                    if (text == "battery")
                    {
                        string b = SystemInformation.PowerStatus.BatteryLifePercent.ToString();
                        text = b.ToString();
                        MessageBox.Show(text.ToString(), "CollectorServer");
                    }


                    //останавливаем сервер
                    if (text == "stop")
                    {
                        MessageBox.Show("Сервер будет остановлен!", "CollectorServer");

                        server.Stop();

                        flag = false;
                    }
                }
            }
        }
    }
}
