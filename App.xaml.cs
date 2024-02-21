using System.Configuration;
using System.Data;
using System.Windows;

namespace KursovoiProekt
{
    //обрабатываем Startup с использованием using, вызывать можно только один экземпляр приложения
    public partial class App : Application
    {
        System.Threading.Mutex? mutex;
        private void App_Startup(object sender, StartupEventArgs e)
        {
            bool createdNew;
            string mutName = "Приложение";
            mutex = new System.Threading.Mutex(true, mutName, out createdNew);
            if (!createdNew)
            {
                this.Shutdown();
            }
        }
        public App()
        {
            this.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }

    }

}
