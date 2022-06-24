using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;

using Android.Widget;
using AndroidX.AppCompat.App;

using Java.Lang;
using Android.Graphics;
using System.Collections.Generic;
using System.Collections;
using Android.Content;

using System.IO;


using Java.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AndroidX.RecyclerView.Widget;
using AndroidX.ViewPager.Widget;
using Google.Android.Material.Tabs;
using AndroidX.Core.App;
using Java.Util;
using Xamarin.Forms;

namespace BurrLife
{


    public struct Timer
    {
        public TextView l1;
        public TextView time;
        public TextView date;
        public TextView hour;
        public TextView minut;
        public TextView seconds;
        public LinearLayout timerLayout;

    }

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false, HardwareAccelerated = true)]


    public class MainActivity : AppCompatActivity, DialogTodo.ItemClickListener,RoutineCreate.ItemClickListener1
    {
        
        
        private ViewPager pager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            TextView title = FindViewById<TextView>(Resource.Id.textView1);
            setCustomFont(title, "INTRO.OTF");
            TabLayout tab = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            ViewPager pager = FindViewById<ViewPager>(Resource.Id.viewPager0);
            pager.Adapter = new MainPagerAdapter(this.SupportFragmentManager);
            pager.SetCurrentItem(1, false);
            tab.SetupWithViewPager(pager);

            AlarmManager manager = (AlarmManager)this.GetSystemService(Context.AlarmService);
            Calendar calendar = Calendar.GetInstance(Locale.Default);
            calendar.Set(CalendarField.HourOfDay, 18);
            calendar.Set(CalendarField.Minute, 54);
            calendar.Set(CalendarField.Second, 0);

            Intent intent = new Intent(this, typeof(BootReceive));
            PendingIntent pIntent = PendingIntent.GetService(this, 0, intent, PendingIntentFlags.UpdateCurrent);

            //manager.SetExact(AlarmType.RtcWakeup, calendar.TimeInMillis, pIntent);
            manager.SetRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis, 1000 * 60 * 60 * 24, pIntent);


            

            
        }
       
        private void setCustomFont(TextView t, string assetFont)
        {
            var font = Typeface.CreateFromAsset(Assets, assetFont);
            t.Typeface = font;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }




        public void onItemClick(TaskPriorityModel model,TaskAdapter adapter)
        {
            PageActivityies.MainActivity1.modelList.Add(model);
            PageActivityies.MainActivity1.modelList.Sort(new MainActivity.CompareItems());



            PageActivityies.MainActivity1._used.Clear();
            for (int i = 0; i < PageActivityies.MainActivity1.modelList.Count && i < 3; i++)
                PageActivityies.MainActivity1._used.Add(PageActivityies.MainActivity1.modelList[i]);

            //TODO: REMOVE SERIALIZE OBJECT FROM THIS
            SerializeObject(PageActivityies.MainActivity1.path, PageActivityies.MainActivity1.modelList, typeof(List<TaskPriorityModel>));
            adapter.NotifyDataSetChanged();
            PageActivityies.GoalActivity.adapter1.NotifyDataSetChanged();
        }
        public static void SerializeObject(string path,object model, System.Type type)
        {


            XmlSerializer serializer = new XmlSerializer(type);


            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {

                try
                {
                    //serializer.Serialize(stream, modelList);
                    serializer.Serialize(stream, model);
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine("Error : " + ex.Message);
                }
            }

        }

        public void onItemClick1(RoutineTask model, RoutineAdapter adapter)
        {
            MakeRoutine.task.Add(model);
            MakeRoutine.task.Sort(new CompareTimer());
            
            //TODO: REMOVE SERIALIZE OBJECT FROM THIS
            SerializeObject(MakeRoutine.DataPath,MakeRoutine.task, typeof(List<RoutineTask>));
            adapter.NotifyDataSetChanged();


            IList<IParcelable> parse = new List<IParcelable>();
            foreach (var item in MakeRoutine.task) parse.Add((IParcelable)item);
            try
            {
                Intent intent = new Intent(this, typeof(RoutineService));
                StartService(intent);
            }
            catch (Exception e) { }


            
            
        }
      



        public class CompareItems : IComparer<TaskPriorityModel>
        {
            public int Compare(TaskPriorityModel x, TaskPriorityModel y)
            {
                if (x.priority > y.priority) return -1;
                else if (x.priority < y.priority) return 1;
                else
                {
                    if (x.deadline < y.deadline) return 1;
                    else if (x.deadline > y.deadline) return -1;
                    return 0;
                }
            }
        }
        public class CompareTimer : IComparer<RoutineTask>
        {
            public int Compare(RoutineTask x, RoutineTask y)
            {
                if (x.getTime() < y.getTime()) return 1;
                else if (x.getTime() > y.getTime()) return -1;
                return 0;
            }
        }

        //toDo remove


    }
}

