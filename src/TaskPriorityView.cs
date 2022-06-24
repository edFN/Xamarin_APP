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
    class TaskPriorityView
    {
        public virtual void printTask(TaskPriorityModel model)
        {
            Console.WriteLine("Task name: " + model.taskName);
            Console.WriteLine("Task Desription: " + model.taskDescription);
            Console.WriteLine("Task Deadline: " + model.deadline);
            Console.Write("Task Priority: ");
            if (model.priority == 2) Console.WriteLine("HIGH");
            else if (model.priority == 1) Console.WriteLine("MIDDLE");
            else if (model.priority == 0) Console.WriteLine("LOW");
        }
    }
}