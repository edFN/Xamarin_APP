using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomSheet;
using Google.Android.Material.TextField;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using static Android.Widget.CalendarView;

namespace BurrLife
{
    class DialogTodo : BottomSheetDialogFragment, View.IOnClickListener
    {

        private ItemClickListener mListen;
        private DateTime time = DateTime.Now;

        private TaskAdapter adapter;
        public DialogTodo(TaskAdapter adapter)
        {
            this.adapter = adapter;
        }


        public static DialogTodo newInstance(TaskAdapter a)
        {
            return new DialogTodo(a);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.adder, container, false);
        }
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            view.FindViewById(Resource.Id.button1).SetOnClickListener(this);
            var d = view.FindViewById<CalendarView>(Resource.Id.calendarView1);
            d.DateChange += CalendarOnDateChange;

        }


        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            if (context is ItemClickListener)
            {
                mListen = (ItemClickListener)context;
            }
            else
            {
                throw new Exception("Error");
            }
        }

        public override void OnDetach()
        {
            mListen = null;
            base.OnDetach();
        }

        public void OnClick(View v)
        {
            var textInputEditText1 = View.FindViewById<TextInputEditText>(Resource.Id.textInputEditText1);
            var name = View.FindViewById<TextInputEditText>(Resource.Id.textInputEditText1).Text;
            var description = View.FindViewById<EditText>(Resource.Id.editText1).Text;
            var priority = View.FindViewById<RadioGroup>(Resource.Id.radioGroup1).CheckedRadioButtonId;

            if (textInputEditText1.Text == string.Empty)
            {
                textInputEditText1.Text = "Поле не может быть пустым";
                Dismiss();
                return;
            }

            TaskPriorityModel model = new TaskPriorityModel(name, description, time, (byte)priority);

            mListen.onItemClick(model, adapter);

            Dismiss();
        }
        private void CalendarOnDateChange(object sender, CalendarView.DateChangeEventArgs args)
        {
            var newdatetime = new DateTime(args.Year, args.Month, args.DayOfMonth);
            time = newdatetime;

        }

        public interface ItemClickListener
        {
            void onItemClick(TaskPriorityModel model, TaskAdapter adapter);
        }


    }
    public class RoutineCreate : BottomSheetDialogFragment, View.IOnClickListener
    {

        private ItemClickListener1 mListen;
        private DateTime time = DateTime.Now;

        private RoutineAdapter adapter;
        public RoutineCreate(RoutineAdapter adapter)
        {
            this.adapter = adapter;
        }


        public static RoutineCreate newInstance(RoutineAdapter a)
        {
            return new RoutineCreate(a);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.adder1, container, false);
        }
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            view.FindViewById(Resource.Id.button1).SetOnClickListener(this);


        }


        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            if (context is ItemClickListener1)
            {
                mListen = (ItemClickListener1)context;
            }
            else
            {
                throw new Exception("Error");
            }
        }

        public override void OnDetach()
        {
            mListen = null;
            base.OnDetach();
        }

        public void OnClick(View v)
        {

            //name
            var name = View.FindViewById<TextInputEditText>(Resource.Id.textInputEditText1).Text;
            var time = View.FindViewById<TimePicker>(Resource.Id.timePicker1);

            if (name == string.Empty)
            {
                Toast.MakeText(Context.ApplicationContext, "Поле не может быть пустым", ToastLength.Short);
                Dismiss();
                return;
            }

            RoutineTask model = new RoutineTask(name, new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,time.Hour,time.Minute,0));

            mListen.onItemClick1(model, adapter);

            Dismiss();
        }


        public interface ItemClickListener1
        {
            void onItemClick1(RoutineTask model, RoutineAdapter adapter);
        }

    }
}