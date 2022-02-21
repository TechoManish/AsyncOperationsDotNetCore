using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncExplaination
{
    public class MainClass
    {
        static async Task Main(string[] args)
        {
            DateTime startTime;
            DateTime endTime;
            Console.WriteLine("Press Enter to go synchronous, any other key for async");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                Console.WriteLine("Synchronity... Timer starts:{0}" + DateTime.Now); startTime =DateTime.Now ;
                SyncOperations(2);
                endTime = DateTime.Now;
                double millisecondsTimeTaken = (endTime - startTime).TotalMilliseconds ;
                Console.WriteLine("TIME Taken (ms): " + millisecondsTimeTaken);
            }
            else {
                Console.WriteLine();
                Console.WriteLine("Asynchronity...Timer starts:{0}" + DateTime.Now); startTime = DateTime.Now;
                await AsyncOperations(2);
                endTime = DateTime.Now;
                double millisecondsTimeTaken = (endTime - startTime).TotalMilliseconds;
                Console.WriteLine("TIME Taken (ms): " + millisecondsTimeTaken);
            }

        }

        #region Always sync methods : these would be executed v fast.
        private static Juice PourOJ()
        {
            Console.WriteLine("Pouring orange juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) =>
            Console.WriteLine("Putting jam on the toast");

        private static void ApplyButter(Toast toast) =>
            Console.WriteLine("Putting butter on the toast");


        private static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }
        #endregion

        #region Async approach : make the longer running methods as async and instead of blocking , lets await


        static async Task AsyncOperations(int count)
        {
            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            var eggsTask =  FryEggsAsync(2);
            var baconTask =  FryBaconAsync(3);
            var toastTask =  MakeToastWithButterAndJamAsync(2);

            var breakfastTasks = new List<Task> { eggsTask, baconTask, toastTask };

            // Async Approach 1 - wait for all of the tasks to finish.. Is this a good approach?
            //await Task.WhenAll(breakfastTasks);
            //Console.WriteLine("eggs are ready");
            //Console.WriteLine("bacon is ready");
            //Console.WriteLine("toast is ready");


            //Async Approach 2: Announce when any of the breakfasts async tasks complete

            while (breakfastTasks.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(breakfastTasks);
                if (finishedTask == eggsTask)
                {
                    Console.WriteLine("eggs are ready");
                }
                else if (finishedTask == baconTask)
                {
                    Console.WriteLine("bacon is ready");
                }
                else if (finishedTask == toastTask)
                {
                    Console.WriteLine("toast is ready");
                }
                breakfastTasks.Remove(finishedTask);
            }

            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            Console.WriteLine("Breakfast is ready!");
        }

        static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
        {
            var toast = await ToastBreadAsync(number);
            ApplyButter(toast);
            ApplyJam(toast);

            return toast;
        }

        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            await Task.Delay(3000);
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            await Task.Delay(3000);
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            await Task.Delay(3000);
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        private static async Task<Egg> FryEggsAsync(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            await Task.Delay(3000);
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            await Task.Delay(3000);
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }

        #endregion

        #region Sync approach : no matter how long the operations we execute them synchronously Huh!!

        static void SyncOperations(int count)
        {
            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            MakeToastWithButterAndJam(count);
            FryBacon(count);
            FryEggs(count);
             Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            Console.WriteLine("Breakfast is ready!");

        }
        static Toast MakeToastWithButterAndJam(int number)
        {
            var toast =  ToastBread(number);
            ApplyButter(toast);
            ApplyJam(toast);

            return toast;
        }

        private static Toast ToastBread(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static Bacon FryBacon(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            Task.Delay(3000).Wait();
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        private static Egg FryEggs(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            Task.Delay(3000).Wait();
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }


        #endregion
    }

    public class AsynchronousTask
    {

        static async Task Main2()
        {

            DateTime startTime;
            DateTime endTime;
           
            Console.WriteLine("Synchronity... Timer starts:{0}" + DateTime.Now); startTime = DateTime.Now;
            

            var task1 =  LongRunningMethod1Async();
           var task2 =   LongRunningMethod2Async();
          var task3 =    LongRunningMethod3Async();
            var asyncTasksParallel = new List<Task> { task1, task2, task3 };
            while (asyncTasksParallel.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(asyncTasksParallel);
                if (finishedTask == task1)
                {
                    Console.WriteLine("task1 completed");
                }
                else if (finishedTask == task2)
                {
                    Console.WriteLine("task2 completed");
                }
                else if (finishedTask == task3)
                {
                    Console.WriteLine("task3 completed");
                }
                asyncTasksParallel.Remove(finishedTask);
            }


            endTime = DateTime.Now;
            double millisecondsTimeTaken = (endTime - startTime).TotalMilliseconds;
            Console.WriteLine("TIME Taken (ms): " + millisecondsTimeTaken);
        }

        static async Task LongRunningMethod1Async()
        {
            await Task.Delay(2000);
            Console.WriteLine("done waiting...");
        }
        static async Task LongRunningMethod2Async()
        {
            await Task.Delay(2000);
            Console.WriteLine("done waiting...");
        }
        static async Task LongRunningMethod3Async()
        {
            await Task.Delay(2000);
            Console.WriteLine("done waiting...");
        }
        //static void LongRunningMethod2()
        //{
        //    Task.Delay(2000).Wait();
        //}
        //static void LongRunningMethod3()
        //{
        //    Task.Delay(2000).Wait();
        //}

    }

    public class SynchronousTaskRemodelled
    {

        static async Task Main1()
        {

            DateTime startTime;
            DateTime endTime;

            Console.WriteLine("Synchronity... Timer starts:{0}" + DateTime.Now); startTime = DateTime.Now;

            ////calling async methods in sync
            //await LongRunningAsyncMethod1();

            //await LongRunningAsyncMethod2();

            //await  LongRunningAsyncMethod3();

            //call long running method:
            var task1 = LongRunningAsyncMethod1();
            var task2 = LongRunningAsyncMethod2();
            var task3 = LongRunningAsyncMethod3();


            await task1;
            await task2;
            await task3;

            //await Task.WhenAll(task1, task2, task3);




            #region async calls with taskss

            //var asyncTasksParallel = new List<Task> { task1, task2, task3, task4, task5 };
            // while (asyncTasksParallel.Count > 0)
            // {
            //     Task finishedTask = await Task.WhenAny(asyncTasksParallel);
            //     if (finishedTask == task1)
            //     {
            //         Console.WriteLine("task1 completed");
            //     }
            //     else if (finishedTask == task2)
            //     {
            //         Console.WriteLine("task2 completed");
            //     }
            //     else if (finishedTask == task3)
            //     {
            //         Console.WriteLine("task3 completed");
            //     }
            //     else if (finishedTask == task4)
            //     {
            //         Console.WriteLine("task4 completed");
            //     }
            //     else if (finishedTask == task5)
            //     {
            //         Console.WriteLine("task5 completed");
            //     }
            //     asyncTasksParallel.Remove(finishedTask);
            // }
            #endregion

            endTime = DateTime.Now;
            double millisecondsTimeTaken = (endTime - startTime).TotalMilliseconds;
            Console.WriteLine("Complete TIME Taken (ms) for the whole Main method: " + millisecondsTimeTaken);
        }

        static async Task LongRunningAsyncMethod1()
        {
          await  Task.Delay(2000);  
            Console.WriteLine("done waiting...Operation1 completed at " + System.DateTime.Now.ToLongTimeString());
        }

        static async Task LongRunningAsyncMethod2()
        {
            await Task.Delay(6000);
            Console.WriteLine("done waiting...Operation2 completed at "  + System.DateTime.Now.ToLongTimeString());
        }

        static async Task LongRunningAsyncMethod3()
        {
            await Task.Delay(4000);
            Console.WriteLine("done waiting...Operation3 completed at " + System.DateTime.Now.ToLongTimeString());
        }

    }

    public class SynchronousTask
    {

        static async Task SyncTaskMethod()
        {

            DateTime startTime;
            DateTime endTime;

            Console.WriteLine("ASynchronity... Timer starts:{0}" + DateTime.Now); startTime = DateTime.Now;

            var task1 =  LongRunningAsyncMethod1();
            var task2 =  LongRunningAsyncMethod2();
            var task3 = LongRunningAsyncMethod3();

            await task1;
            await task2;
            await task3;

            endTime = DateTime.Now;
            double millisecondsTimeTaken = (endTime - startTime).TotalMilliseconds;
            Console.WriteLine("Complete TIME Taken (ms) for the whole Main method: " + millisecondsTimeTaken);
        }

        static async Task LongRunningAsyncMethod1()
        {
             await Task.Delay(2000);
            Console.WriteLine("done waiting...Operation1 completed at " + System.DateTime.Now.ToLongTimeString());
        }
        static async Task LongRunningAsyncMethod2()
        {
            await Task.Delay(6000);
            Console.WriteLine("done waiting...Operation2 completed at " + System.DateTime.Now.ToLongTimeString());
        }
        static async Task LongRunningAsyncMethod3()
        {
            await Task.Delay(4000);
            Console.WriteLine("done waiting...Operation3 completed at " + System.DateTime.Now.ToLongTimeString());
        }



    }

}
