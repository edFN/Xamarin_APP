using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using String = Java.Lang.String;

namespace BurrLife
{
    [Service]
    class RoutineService : Service
    {

        [return: GeneratedEnum]

        private Thread th;
        private NotificationManager notManager;
        private List<RoutineTask> tasks;
        private static String TAG = new String("ExampleService");


        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            if (flags == StartCommandFlags.Retry)
            {
                try
                {

                    System.Collections.IList d = intent.Extras.GetParcelableArrayList("Data");

                    List<RoutineTask> x = new List<RoutineTask>();
                    foreach (var item in d)
                    {
                        x.Add(((RoutineTask)item));
                    }
                    th.Interrupt();
                    tasks = x;


                }
                catch (System.Exception e)
                {

                    Toast.MakeText(this.ApplicationContext, $"Error {e.Message}", ToastLength.Long);
                    tasks = new AsyncGenerateRoutine().Execute(MakeRoutine.DataPath).GetResult();
                    if (tasks == null || tasks.Count == 0) StopSelf();

                }
            }
            else
            {

                tasks = new AsyncGenerateRoutine().Execute(MakeRoutine.DataPath).GetResult();
                if (tasks == null || tasks.Count == 0) StopSelf();
            }
            createNotificationThread();

            return StartCommandResult.Sticky;
        }

        public override void OnCreate()
        {
            Console.WriteLine("Service создан тести!!!");
            notManager = (NotificationManager)GetSystemService(Context.NotificationService);

            Toast.MakeText(this, "Сервис создан", ToastLength.Short).Show();
            base.OnCreate();
        }

        private void createNotificationThread()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            PendingIntent pIntent = PendingIntent.GetActivity(this, 0, intent, 0);
            var builderString = new System.Text.StringBuilder();
            RoutineTask last = new RoutineTask();

            Runnable runnable = new Runnable(() =>
            {

                while (!th.IsInterrupted)
                {
                    try
                    {
                        var date = DateTime.Now;



                        foreach (var task in tasks)
                        {
                            if (date.Hour == task.time.Hour && date.Minute == task.time.Minute && date.Second < 2 && task != last)
                            {
                                last = task;
                                builderString.AppendLine($"Пора сделать {task.taskName}");
                            }
                        }


                        if (builderString.Length != 0)
                        {
                            NotificationCompat.Builder builder =
                           new NotificationCompat.Builder(this, "Flowerbloom")
                            .SetSmallIcon(Resource.Drawable.ic_notifications_black_24dp)
                            .SetContentTitle("Рутина")
                            .SetContentText(builderString.ToString()).SetContentIntent(pIntent);


                            Notification notification = builder.Build();
                            createChannel(notManager);
                            builderString.Clear();
                            notManager.Notify(41, notification);
                        }
                        Thread.Sleep(200);
                    }
                    catch (InterruptedException e) { break; }
                }
            });
            th = new Thread(runnable);
            th.Start();
        }

        public override void OnDestroy()
        {

            
            base.OnDestroy();

        }

        private static void createChannel(NotificationManager nm)
        {
            //Android.App.BuildVersionCodes
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Base)
            {
                NotificationChannel channel = new NotificationChannel("Flowerbloom", "Flowerbloom", Android.App.NotificationImportance.Default);
                nm.CreateNotificationChannel(channel);
            }
        }



        public override IBinder OnBind(Intent intent)
        {
            return null;
        }


    }
}