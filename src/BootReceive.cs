using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurrLife
{
    [BroadcastReceiver(Enabled =true, Exported =false)]
    [IntentFilter(new string[]
    {
        Intent.ActionBootCompleted,
        Intent.CategoryDefault
        
    })]
    
    [IntentFilter(actions: new string[]
    {
        Intent.ActionBootCompleted
    })]
    class BootReceive : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
           if(intent.Action == Intent.ActionBootCompleted)
            {
                var data = new AsyncGenerateRoutine().Execute(MakeRoutine.DataPath).GetResult();
                if (data.Count == 0) return;
                IList<IParcelable> parse = new List<IParcelable>();
                foreach (var item in data) parse.Add((IParcelable)item);
                try
                {
                    Intent i= new Intent(context, typeof(RoutineService));
                    context.StartService(intent);
                }
                catch (Exception e) { }

            }

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
    }
}