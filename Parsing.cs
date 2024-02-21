//парсинг данных
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Windows;
using Microsoft.Extensions.Options;
using System.IO;


namespace KursovoiProekt
{
    public class Parsing
    {
        internal static readonly string[] Strana = ["Соединенные Штаты", "Канада", "Великобритания", "Швейцария", "Япония", "Австралия", "Новая Зеландия"];
        internal static readonly double[] Nazv = new double[35];
        internal static readonly string[] ValPara = ["USD/CAD", "GBP/USD", "USD/CHF", "USD/JPY", "AUD/USD",  "NZD/USD", "GBP/CAD", "CAD/CHF", "CAD/JPY", "AUD/CAD",
            "NZD/CAD", "GBP/CHF", "GBP/JPY", "GBP/AUD", "GBP/NZD", "CHF/JPY", "AUD/CHF", "NZD/CHF", "AUD/JPY", "NZD/JPY", "AUD/NZD"];
        internal static readonly string[] Futures = ["US 30", "FTSE 100", "Швейцария 20", "Япония 225", "Австралия 200"];
        internal static readonly string[] Futures1 = ["CATSX", "NZX 50"];

        public static void ReadNewData()
        {
            string[] fileEntries = Directory.GetFiles(@"C:\Program Files\Google", "chrome.exe", SearchOption.AllDirectories);
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            //указываем путь расположения нашего браузера chrome 
            ChromeOptions options = new()
            {
                BinaryLocation = fileEntries[0],
                PageLoadStrategy = PageLoadStrategy.Eager
            };
            //задаём свойства браузера
            options.AddArguments(
                      "--headless=new",
                      "user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
                    );
            //задаём драйвер браузера
            ChromeDriver driver = new(driverService, options);
            //подключаемся к нужной странице
            try
            {
                driver.Navigate().GoToUrl("https://ru.tradingeconomics.com/bonds");
                //ищем нужные элементы на странице, значение бондов
                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> znacheniebonda = driver.FindElements(By.XPath("//*[@id='p']"));
                //название стран
                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> nazvaniestrany = driver.FindElements(By.TagName("b"));
                //извлекаем данные из коллекций для создания экземпляра класса для записи в базу данных 
                for (int i = 0; i < Strana.Length; i++)
                {
                    for (int y = 0; y < nazvaniestrany.Count; y++)
                    {
                        if (Strana[i] == nazvaniestrany[y].Text)
                        {
                            Nazv[i] = Convert.ToDouble(znacheniebonda[y].Text.Replace(".", ","));
                            break;
                        }

                    }
                }


            }
            finally { driver.Quit(); }
        }
        public static void ReadNewData1()
        {
            string[] fileEntries1 = Directory.GetFiles(@"C:\Program Files\Google", "chrome.exe", SearchOption.AllDirectories);
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            ChromeOptions options1 = new()
            {
                BinaryLocation = fileEntries1[0],
                PageLoadStrategy = PageLoadStrategy.Eager
            };
            options1.AddArguments(
               "--headless=new",
               "user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
            );
            //задаём драйвер браузера
            ChromeDriver driver1 = new(driverService, options1);
            //подключаемся к нужной странице
            try
            {
                driver1.Navigate().GoToUrl("https://ru.investing.com/currencies/streaming-forex-rates-majors");
                //получаем массив значений валютных пар
                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> ParaZnach = driver1.FindElements(By.XPath("//td[3][@class=\"datatable-v2_cell__IwP1U dynamic-table-v2_col-other__zNU4A text-right rtl:text-right\"]"));
                //получаем массив наименований валютных пар
                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> ParaNazvanie = driver1.FindElements(By.XPath("//span[@class=\"block ml-3 text-ellipsis overflow-hidden whitespace-nowrap\"]"));
                //извлекаем из коллекции валютные пары и их значения
                for (int i = 0; i < ValPara.Length; i++)
                {
                    for (int y = 0; y < ParaNazvanie.Count; y++)
                    {

                        if (ValPara[i] == ParaNazvanie[y].Text)
                        {
                            Nazv[i + 7] = Convert.ToDouble(ParaZnach[y].Text.Replace(".", ","));
                            break;
                        }
                    }
                }
            }
            finally { driver1.Quit(); }
        }
        public static void ReadNewData2()
        {
            string[] fileEntries2 = Directory.GetFiles(@"C:\Program Files\Google", "chrome.exe", SearchOption.AllDirectories);
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            ChromeOptions options2 = new()
            {
                BinaryLocation = fileEntries2[0],
                PageLoadStrategy = PageLoadStrategy.Eager
            };
            options2.AddArguments(
               "--headless=new",
               "user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
            );
            //задаём драйвер браузера
            ChromeDriver driver2 = new(driverService, options2);
            //подключаемся к нужной странице
            try
            {
                driver2.Navigate().GoToUrl("https://ru.investing.com/indices/indices-futures");
                //получаем массив значений фьючерсов
                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> FuturesZnach = driver2.FindElements(By.XPath ("//td[4][@class=\"datatable_cell__LJp3C datatable_cell--align-end__qgxDQ table-browser_col-last__LeSgu\"]"));
                //получаем массив наименований фьючерсов
                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> FuturesNazvanie = driver2.FindElements(By.XPath("//a[@class=\"text-secondary hover:underline focus:underline hover:text-secondary focus:text-secondary font-bold datatable_cell--name__link__2xqgx\"]"));
                //извлекаем из коллекции фьючерсы и их значения
                for (int i = 0; i < Futures.Length; i++)
                {
                    for (int y = 0; y < FuturesNazvanie.Count; y++)
                    {

                        if (Futures[i] == FuturesNazvanie[y].Text)
                        {
                            Nazv[i + 28] = Convert.ToDouble(FuturesZnach[y].Text.Replace(".", ""));
                            break;
                        }
                    }
                }
            }
            finally { driver2.Quit(); }
        }
        public static void ReadNewData3()
        {
            string[] fileEntries3 = Directory.GetFiles(@"C:\Program Files\Google", "chrome.exe", SearchOption.AllDirectories);
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            ChromeOptions options3 = new()
            {
                BinaryLocation = fileEntries3[0],
                PageLoadStrategy = PageLoadStrategy.Eager
            };
            options3.AddArguments(
               "--headless=new",
               "user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
            );
            //задаём драйвер браузера
            ChromeDriver driver3 = new(driverService, options3);
            //подключаемся к нужной странице
            try
            {
                driver3.Navigate().GoToUrl("https://ru.tradingeconomics.com/stocks");
                //получаем массив значений фьючерсов
                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> FuturesZnach1 = driver3.FindElements(By.XPath("//td[3][@id='p']"));
                //получаем массив наименований фьючерсов
                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> FuturesNazvanie1 = driver3.FindElements(By.XPath("//td[2][@class=\"datatable-item-first\"]"));
                //извлекаем из коллекции фьючерсы и их значения
                for (int i = 0; i < Futures1.Length; i++)
                {
                    for (int y = 0; y < FuturesNazvanie1.Count; y++)
                    {

                        if (Futures1[i] == FuturesNazvanie1[y].Text)
                        {
                            Nazv[i + 33] = Convert.ToDouble(FuturesZnach1[y].Text.Replace(".", ","));
                            break;
                        }
                    }
                }
            }
            finally { driver3.Quit(); }
        }
        public static void Newbond()
        {
            //определяем переменную для текущего времени
            DateTime dateTime = DateTime.Now;
            using ApplicationContext db = new();
            //создаем экземпляр класса для БД
            Bond BBB = new()
            {
                Chislo = dateTime.ToString("g"),
                US = Nazv[0],
                CA = Nazv[1],
                GB = Nazv[2],
                CF = Nazv[3],
                JP = Nazv[4],
                AU = Nazv[5],
                NZ = Nazv[6],
                USDCAD = Nazv[7],
                GBPUSD = Nazv[8],
                USDCHF = Nazv[9],
                USDJPY = Nazv[10],
                AUDUSD = Nazv[11],
                NZDUSD = Nazv[12],
                GBPCAD = Nazv[13],
                CADCHF = Nazv[14],
                CADJPY = Nazv[15],
                AUDCAD = Nazv[16],
                NZDCAD = Nazv[17],
                GBPCHF = Nazv[18],
                GBPJPY = Nazv[19],
                GBPAUD = Nazv[20],
                GBPNZD = Nazv[21],
                CHFJPY = Nazv[22],
                AUDCHF = Nazv[23],
                NZDCHF = Nazv[24],
                AUDJPY = Nazv[25],
                NZDJPY = Nazv[26],
                AUDNZD = Nazv[27],
                US30 = Nazv[28],
                FTSE100 = Nazv[29],
                SMI = Nazv[30],
                JP225 = Nazv[31],
                AUS200 = Nazv[32],
                CACash = Nazv[33],
                NZ50 = Nazv[34]
            };
            // добавляем их в бд
            _ = db.Bonds.Add(BBB);
            _ = db.SaveChanges();


        }
    }
}
