using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace TestProjCrossInform
{
    class Program
    {

        private static void Main(string[] args)
        {
            
            if (args.Length < 1)
            {
                Console.WriteLine("Файл не подан через командную строку");
                return;
            }

            var fileName = args[0];

            if (!File.Exists(fileName))
            {
                Console.WriteLine("Файл не исуществует");
                return;
            }

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            Task task1 = new Task(() => UtilityClass.Run(fileName,token));
            task1.Start();
            try
            {
                do
                {
                    var c = Console.ReadKey(true);
                    if(task1.IsCompleted) break;
                    tokenSource.Cancel();
                } while (task1.IsCompleted && !task1.IsCanceled);

                task1.Wait();
            }


            catch (AggregateException ae)
            {
                foreach (Exception e in ae.InnerExceptions)
                {
                    if (e is TaskCanceledException)
                        Console.WriteLine("Unable to compute mean: {0}",
                            ((TaskCanceledException) e).Message);
                    else
                        Console.WriteLine("Exception: " + e.GetType().Name);
                }
            }


            



                
            }

        }
    }
