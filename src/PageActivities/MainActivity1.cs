
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using Java.Interop;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace BurrLife.PageActivityies
{


    public class MainActivity1 : Fragment    {
        private Thread th;
        private TextView Title;
        private Timer t;
        public static  TaskAdapter adapter;
        private Button btn;
        private TextView textViewT;
        private ListView recyclerView1;
        public static List<TaskPriorityModel> modelList;
        public static List<TaskPriorityModel> _used = new List<TaskPriorityModel>();
        private FragmentActivity myContext;
        public static string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
           "datax.xml");

        private bool isActive = true;

        public override void OnAttach(Android.App.Activity activity)
        {
            myContext = (FragmentActivity)activity;
            startTimer(new LeftForNextDay());
            isActive = true;
            base.OnAttach(activity);
        }


        
        public override void OnDetach()
        {
      
            try
            {
                th.Interrupt();
                
            }
            catch (Java.Lang.Exception e) { }  
            base.OnDetach();
        }
        public override void OnDestroy()
        {
            isActive = false;
            base.OnDestroy();
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.viewMain, container, false);
            modelList = new AsyncGenerateList().Execute(path).GetResult();
            for (int i = 0; i < modelList.Count && i < 3; i++) _used.Add(modelList[i]);
            init(view);
            return view;
        }

        private void startTimer(ITimeCalculate calc)
        {
            Runnable setTime = new Runnable(() =>
            {
                while (!th.IsInterrupted)
                {
                    this.Activity.RunOnUiThread(new Runnable(() => this.t.time.Text = calc.timeCalculate()));
                    try
                    {
                        Thread.Sleep(400);
                    }
                    catch (InterruptedException)
                    {
                        break;
                    }
                }
            });
            th = new Thread(setTime);
            th.Start();
        }

        private void init(View view)
        {
            // Title = view.FindViewById<TextView>(Resource.Id.textView1);

            //TimerViews
            t.l1 = view.FindViewById<TextView>(Resource.Id.textView2);
            t.time = view.FindViewById<TextView>(Resource.Id.textView3);
            t.hour = view.FindViewById<TextView>(Resource.Id.textView4);
            t.minut = view.FindViewById<TextView>(Resource.Id.textView5);
            t.seconds = view.FindViewById<TextView>(Resource.Id.textView6);
            t.date = view.FindViewById<TextView>(Resource.Id.textView7);

            //TODO: Создать кнопку, а пока пусть будет layout

            textViewT = view.FindViewById<TextView>(Resource.Id.textView8);
            recyclerView1 = view.FindViewById<ListView>(Resource.Id.listView1);
            setFonts();
            btn = view.FindViewById<Button>(Resource.Id.button1);

            TextView element = view.FindViewById<TextView>(Resource.Id.textView9);

            element.Click += (o, e) =>
            {
                Intent intent = new Intent(this.Context, typeof(BurrLife.Resources.layout.Timers));
                StartActivity(intent);
            };

            
            adapter = new TaskAdapter(this.Context, _used);
            
            recyclerView1.SetAdapter(adapter);
            
            recyclerView1.ItemClick += delegate (object o, AdapterView.ItemClickEventArgs e)
            {
                Intent intent = new Intent(this.Context, typeof(TaskInfoActivity));
                intent.PutExtra("ID", e.Position);
                intent.PutExtra("Name", modelList[e.Position].taskName);
                intent.PutExtra("Description", modelList[e.Position].taskDescription);
                intent.PutExtra("Deadline",modelList[e.Position].deadline.Ticks);
                StartActivityForResult(intent,0);
            };
            
            
            recyclerView1.ItemLongClick += delegate (object sender, AdapterView.ItemLongClickEventArgs args)
            {
                AlertDialog.Builder alertBuilder = new AlertDialog.Builder(Context);
                alertBuilder.SetTitle("Удаление задачи").
                SetMessage($"Удалить задачу '{modelList[args.Position].taskName}' ?").
                SetPositiveButton("Да", (object o, DialogClickEventArgs cargs) =>
                 {
                     modelList.RemoveAt(args.Position);

                     PageActivityies.MainActivity1._used.Clear();
                         for (int i = 0; i < PageActivityies.MainActivity1.modelList.Count && i < 3; i++)
                             PageActivityies.MainActivity1._used.Add(PageActivityies.MainActivity1.modelList[i]);
                     
                     adapter.NotifyDataSetChanged();
                     PageActivityies.GoalActivity.adapter1.NotifyDataSetChanged();
                    
                     MainActivity.SerializeObject(path, modelList, typeof(List<TaskPriorityModel>));


                 }).SetNegativeButton("Нет", (o, e) => { });
                alertBuilder.Show();

             
            };

            
          




            btn.Click += (o, e) =>
            {
                
                showBottomSheet(btn);
            };


           

        }
        public void showBottomSheet(View view)
        {
            DialogTodo addPhotoBottomDialogFragment =
                DialogTodo.newInstance(adapter);
            addPhotoBottomDialogFragment.Show(this.FragmentManager, "TAG_VIEW");

        }
        private void setCustomFont(TextView t, string assetFont)
        {
            var font = Typeface.CreateFromAsset(this.Context.Assets, assetFont);
            t.Typeface = font;
        }
        private void setFonts()
        {


            setCustomFont(t.l1, "INTRO.OTF");

            setCustomFont(t.time, "INTRO.OTF");

            setCustomFont(t.hour, "INTRO.OTF");

            setCustomFont(t.minut, "INTRO.OTF");

            setCustomFont(t.seconds, "INTRO.OTF");

            setCustomFont(textViewT, "INTRO.OTF");
            setCustomFont(t.date, "INTRO.OTF");
        }

       
       
    }

 

}
    