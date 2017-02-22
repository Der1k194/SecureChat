using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;

public class ChatHub : Hub
{
    static List<Users> Users = new List<Users>();

    // Отправка сообщений
    public void Send(string name, string message)
    {
        Clients.All.addMessage(name, message);
    }

    // Подключение нового пользователя
    public void Connect(string userName)
    {
        var id = Context.ConnectionId;


        if (!Users.Any(x => x.ConnectionId == id))
        {
            Users.Add(new Users { ConnectionId = id, Name = userName });

            // Посылаем сообщение текущему пользователю
            Clients.Caller.onConnected(id, userName, Users);

            // Посылаем сообщение всем пользователям, кроме текущего
            Clients.AllExcept(id).onNewUserConnected(id, userName);
        }
    }

    // Отключение пользователя
    public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
    {
        var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        if (item != null)
        {
            Users.Remove(item);
            var id = Context.ConnectionId;
            Clients.All.onUserDisconnected(id, item.Name);
        }

        return base.OnDisconnected(stopCalled);
    }
}
