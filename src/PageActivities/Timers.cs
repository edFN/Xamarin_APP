using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurrLife.Resources.layout
{
    [Activity(Label = "Timers")]
    public class Timers : Activity
    {


        private Thread thread;
        private TextView[] textViews = new TextView[3];
      
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AllTimerrs);

            
            textViews[0] = FindViewById<TextView>(Resource.Id.textView1);
            textViews[1] = FindViewById<TextView>(Resource.Id.textView2);
            textViews[2] = FindViewById<TextView>(Resource.Id.textView3);

            T();
        }

       
        protected override void OnResume()
        {
            if(thread != null && !thread.IsAlive)
            {
                T();
            }
            base.OnResume();
        }
        protected override void OnPause()
        {
            //th.Dispose();
            try
            {
                thread.Interrupt();
            }
            catch (Java.Lang.Exception e) { }
            base.OnPause();
        }

        public void T()
        {
            Runnable run = new Runnable(() =>
            {
                ITimeCalculate[] calculators = new ITimeCalculate[3]
                {
                    new LeftForNextMonth(),new LeftForConcreteDay(new DateTime(DateTime.Now.Year,9,1,0,0,0)),
                    new LeftForNextYear()
                };
                while (!thread.IsInterrupted)
                {
                    RunOnUiThread(() =>
                    {
                        for (int i = 0; i < 3; i++)
                            textViews[i].Text = calculators[i].timeCalculate();
                    });
                    try
                    {
                        Thread.Sleep(200);
                    }
                    catch (InterruptedException)
                    {
                        break;
                    }

                }
            });
            thread = new Thread(run);
            thread.Start();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}