using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;


namespace KursovoiProekt
{
    public partial class ViewModel : ObservableObject
    {
        //свойство Series
        public required ObservableCollection<LiveChartsCore.ISeries> Series { get; set; }
        public required ObservableCollection<LiveChartsCore.ISeries> Series1 { get; set; }
        /////коллекции значений
        ///первая вкладка USDCAD
        public ObservableCollection<ObservableValue> values = [];
        public ObservableCollection<ObservableValue> values1 = [];
        ///вторая вкладка GBFUSD
        public ObservableCollection<ObservableValue> values2 = [];
        public ObservableCollection<ObservableValue> values3 = [];

        //создаём объект-заглушку
        public object Sync { get; } = new();
        //вспомогательная переменная для непрерывного цмкла
        public static bool IsReading { get; set; } = true;
        //оси Y и X
        public static List<Axis>? YAxes { get; set; }
        public static List<Axis>? XAxes { get; set; }
        
        public ViewModel()
        {
            //задаём цвета для шкал
            var blue = new SKColor(25, 118, 210);
            var red = new SKColor(229, 57, 53);
            //коллекции линий с выводом на экран
            //вкладка USDCAD
            Series =
                [
                new LineSeries<ObservableValue> { Values = values, Fill = null, ScalesYAt = 0, ScalesXAt = 0 },
                new LineSeries<ObservableValue> { Values = values1, Fill = null, ScalesYAt = 1 }
                ];
            //вкладка GBPUSD
            Series1 =
                [
                new LineSeries<ObservableValue> { Values = values2, Fill = null, ScalesYAt = 0, ScalesXAt = 0 },
                new LineSeries<ObservableValue> { Values = values3, Fill = null, ScalesYAt = 1 }
                ];

            //контекст данных 
            using (ApplicationContext db = new())
            {    // получаем объекты из бд и передаём нужные значения в коллекцию значений
                ObservableCollection<Bond> users = [.. db.Bonds];
                foreach (Bond u in users)
                {
                    values.Add(new ObservableValue { Value = u.USDCAD + u.US - u.CA });
                    values1.Add(new ObservableValue { Value = u.USDCAD });
                    values2.Add(new ObservableValue { Value = u.GBPUSD + u.US - u.GB });
                    values3.Add(new ObservableValue { Value = u.GBPUSD });
                }
            }
            // шкалa X, выводим последние 70 значений
            int popravka = values.Count;
            if (values.Count > 70) { popravka = 70; }
            XAxes =
            [
                new Axis
                {
                    Name = "Графики",
                    LabelsPaint = new SolidColorPaint(blue),
                    MaxLimit = values.Count,
                    MinLimit = values.Count - popravka
                }
            ];
            //две шкалы Y
            YAxes =
            [
                new Axis
                {
                    Name = "Индикатор",
                    LabelsPaint = new SolidColorPaint(blue),
                },
                new Axis
                {
                    Name = "Валютная пара",
                    LabelsPaint = new SolidColorPaint(red),
                    ShowSeparatorLines = false,
                    Position = LiveChartsCore.Measure.AxisPosition.End
                }
            ];
            //запускаем метод
            _ = ReadNewDataAsync();
        }
        public async Task ReadNewDataAsync()
        {
            await Task.Delay(1000);
            while (IsReading)
            {
                //блокируем возможные потоки
                lock (Sync)
                {
                    int count = 0;
                    while (count == 0)
                    {
                        Parallel.Invoke(Parsing.ReadNewData, Parsing.ReadNewData1);
                        Parallel.Invoke(Parsing.ReadNewData2, Parsing.ReadNewData3);
                        //проверка на правильность полученных данных
                        foreach (double x in Parsing.Nazv)
                        {
                            if (x == 0) { count = 0; break; }
                            else count = 1;
                        }
                    }
                    Parsing.Newbond();
                        //добавляем новые точки из БД  и определяем сигналы для пользователя
                        using (ApplicationContext db = new())
                        {    // получаем объекты из бд и передаём нужные значения в коллекцию значений
                             //n - переменная для размера ряда для сравнения
                            int n = 3;
                            var element = db.Bonds.Max(u => u.Id);
                            var el = (from user in db.Bonds
                                      where user.Id > (element - n)
                                      select user).ToList();
                            //данные USDCAD
                            values.Add(new ObservableValue(el[n - 1].USDCAD + el[n - 1].US - el[n - 1].CA));
                            values1.Add(new ObservableValue(el[n - 1].USDCAD));
                            //данные GBPUSD
                            values2.Add(new ObservableValue(el[n - 1].GBPUSD + el[n - 1].US - el[n - 1].GB));
                            values3.Add(new ObservableValue(el[n - 1].GBPUSD));
                        //прописываем условие для сигнала USDCAD
                             var bondusca1 = el[n-1].US - el[n-1].CA; var bondusca2 = el[n - 2].US - el[n - 2].CA; var bondusca3 = el[n - 3].US - el[n - 3].CA;
                             var usdcad1 = el[n-1].USDCAD; var usdcad2 = el[n - 2].USDCAD; var usdcad3 = el[n - 3].USDCAD;
                             if ((bondusca1 < bondusca2 && bondusca2 < bondusca3 && usdcad1 >= usdcad2 && usdcad2 >= usdcad3) || (bondusca1 >= bondusca2 && bondusca2 >= bondusca3 && usdcad1 < usdcad2 && usdcad2 < usdcad3))
                             {
                                  var addpost = (from user in db.LogPar select user).ToList();
                                  Mail.Milo($"{addpost[0].Email}", "Message from the foreign exchange market", "Please note to USD/CAD");
                             }
                        }
                     //если хотим получать данные из запроса а не из БД
                    // --------------------------------------------------------------------------------------------
                    ////данные USDCAD
                    //values.Add(new ObservableValue(Parsing.Nazv[7] + Parsing.Nazv[0] - Parsing.Nazv[1]));
                    //values1.Add(new ObservableValue(Parsing.Nazv[7]));
                    ////данные GBPUSD
                    //values2.Add(new ObservableValue(Parsing.Nazv[8] + Parsing.Nazv[0] - Parsing.Nazv[2]));
                    //values3.Add(new ObservableValue(Parsing.Nazv[8]));
                    // ---------------------------------------------------------------------------------------------


                }
                //останавливаем поток до начала следующего часа
                DateTime now = DateTime.Now;
                int  minut = 60 - Int32.Parse(now.ToString(":m").TrimStart(':'));
                await Task.Delay(1000 * 60 * minut);
            }
        }

    }
}
  