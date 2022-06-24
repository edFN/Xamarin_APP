
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using Java.Interop;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BurrLife
{


    public class GenericParcelableCreator<T> : Java.Lang.Object, IParcelableCreator where T : Java.Lang.Object, new()
    {
        /// <summary>
        /// Function for the creation of a parcel.
        /// </summary>
        private readonly Func<Parcel, T> _createFunc;

        /// <summary>
        /// Initialize an instance of the GenericParcelableCreator.
        /// </summary>
        public GenericParcelableCreator(Func<Parcel, T> createFromParcelFunc)
        {
            _createFunc = createFromParcelFunc;
        }

        /// <summary>
        /// Create a parcelable from a parcel.
        /// </summary>
        public Java.Lang.Object CreateFromParcel(Parcel parcel)
        {
            return _createFunc(parcel);
        }

        /// <summary>
        /// Create an array from the parcelable class.
        /// </summary>
        public Java.Lang.Object[] NewArray(int size)
        {
            return new T[size];
        }
    }

    [Serializable]
    public class RoutineTask : Java.Lang.Object, IParcelable
    {
        public string taskName { get; set; }
        public DateTime time { get; set; }

        

        public RoutineTask():base()
        {
            taskName = "None";
            time = DateTime.Now.AddHours(1);
        }
        public DateTime getTime ()
        {
            return time;
        }

        public int DescribeContents()
        {
            return 0;
        }
        

        private RoutineTask(Parcel parsel)
        {
            this.taskName = parsel.ReadString();
            this.time = new DateTime(parsel.ReadLong());
            this.time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 
                time.Hour, time.Minute, time.Second);


        }

        

        public void WriteToParcel(Parcel dest, [GeneratedEnum] ParcelableWriteFlags flags)
        {
            dest.WriteString(this.taskName);
            dest.WriteLong(this.time.Ticks);
        }

       
        private static readonly GenericParcelableCreator<RoutineTask> _creator =
            new GenericParcelableCreator<RoutineTask>((parcel) => new RoutineTask(parcel));
        public RoutineTask(string taskName, DateTime time) : base()
        {
            this.time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time.Hour, time.Minute, time.Second);
            this.taskName = taskName;
        }

        [ExportField("CREATOR")]
        public static GenericParcelableCreator<RoutineTask> GetCreator() => _creator;
       
        
        
    }
    class RoutineTaskParcelable : Java.Lang.Object, IParcelable
    {
        public RoutineTask task {get;set;}
        public RoutineTaskParcelable() { }

        private RoutineTaskParcelable(Parcel parcel)
        {
            task = new RoutineTask
            {
                taskName = parcel.ReadString(),
                time = new DateTime(parcel.ReadLong())
            };
        }
        public int DescribeContents()
        {
            return 0;
        }

        public void WriteToParcel(Parcel dest, [GeneratedEnum] ParcelableWriteFlags flags)
        {
            dest.WriteLong(task.time.Ticks);
            dest.WriteString(task.taskName);
        }
        private static readonly GenericParcelableCreator<RoutineTaskParcelable> _creator =
            new GenericParcelableCreator<RoutineTaskParcelable>((parcel) => new RoutineTaskParcelable(parcel));
        [ExportField("CREATOR")]
        public static GenericParcelableCreator<RoutineTaskParcelable> GetCreator() {
            return _creator;
        }
    }


    public class RoutineAdapter : BaseAdapter
    {
        private List<RoutineTask> tasks;    
        private Context context;
        public override int Count => tasks.Count;

        public RoutineAdapter(Context context,List<RoutineTask> tasks)
        {
            this.context = context;
            this.tasks = tasks;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return (RoutineTask)tasks[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            
            View view = convertView;
            var layout = LayoutInflater.From(context);
            if (view == null)
            {
                view = layout.Inflate(Resource.Layout.listCustom, parent, false);
            }

            TextView time = view.FindViewById<TextView>(Resource.Id.time);
            TextView task = view.FindViewById<TextView>(Resource.Id.textItem);
            task.Text = tasks[position].taskName;
            time.Text = tasks[position].time.ToString(@"hh\:mm"); 
            
 
            return view;

        }

    }
    

    public class MakeRoutine : Fragment
    {

        private TextView textView;
        public static List<RoutineTask> task;
        public static string DataPath =  System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
           "data4.xml");
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.Routine, container, false);
            init(view);
            return view;
        }

        public override void OnAttach(Android.App.Activity activity)
        {
            task = new AsyncGenerateRoutine().Execute(DataPath).GetResult();

            base.OnAttach(activity);
        }

        private void init(View v) {
           
             
            RoutineAdapter adapter = new RoutineAdapter(this.Context, task);
            ListView x = v.FindViewById<ListView>(Resource.Id.listView1);
            x.Adapter = adapter;
            Button btn = v.FindViewById<Button>(Resource.Id.button1);

            btn.Click += (o, e) =>
            {
                showBottomSheet(v, adapter);
            };
            x.ItemLongClick += delegate (object sender, AdapterView.ItemLongClickEventArgs args)
            {
                AlertDialog.Builder alertBuilder = new AlertDialog.Builder(Context);
                alertBuilder.SetTitle("Удаление задачи").
                SetMessage($"Удалить задачу '{task[args.Position].taskName}' ?").
                SetPositiveButton("Да", (object o, DialogClickEventArgs cargs) =>
                {
                    task.RemoveAt(args.Position);
                    adapter.NotifyDataSetChanged();
                    MainActivity.SerializeObject(DataPath, task, typeof(List<RoutineTask>));
                    

                }).SetNegativeButton("Нет", (o, e) => { });
                alertBuilder.Show();
            };

            IList<IParcelable> parse = new List<IParcelable>();
            foreach (var item in MakeRoutine.task) parse.Add((IParcelable)item);
            try
            {
                Intent intent = new Intent(this.Context, typeof(RoutineService));
                this.Context.StartService(intent);
            }
            catch (Exception e) { }



        }
       


        public void showBottomSheet(View view,RoutineAdapter adapter)
        {
            RoutineCreate create=
                RoutineCreate.newInstance(adapter);
            create.Show(this.FragmentManager, "TAG_VIEW");

        }
    }
}