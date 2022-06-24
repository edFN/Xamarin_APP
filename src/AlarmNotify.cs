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
    [BroadcastReceiver]
    class AlarmNotify : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var message = intent.Extras.GetString("Name");
            var data = intent.Extras.GetLong("Date"); // in ticks
            var date = new DateTime(data);
            var notIntent = new Intent(context, typeof(MainActivity));

            var contentIntent = PendingIntent.GetActivity(context, 0, notIntent, PendingIntentFlags.CancelCurrent);
            var manager = (NotificationManager)context.GetSystemService(Context.NotificationService);

            var builder = new Notification.Builder(context, "Flowerbloom").SetContentTitle("Рутина").SetSmallIcon(Resource.Drawable.notification_template_icon_bg)
                .SetContentText("Нужно сделать: " + message).SetContentIntent(contentIntent).
                SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis()).SetAutoCancel(true);
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Base)
            {
                NotificationChannel channel = new NotificationChannel("Flowerbloom", "Flowerbloom", Android.App.NotificationImportance.Default);
                manager.CreateNotificationChannel(channel);
            }

            var notification = builder.Build();
            manager.Notify(0, notification);
                
        }
    }
}