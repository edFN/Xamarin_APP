using System;


namespace BurrLife
{

    //implement taskPriorityModel

    [Serializable]
    public class TaskPriorityModel : Java.Lang.Object
    {
        

        public byte priority { get; set; }         
        public string taskName { get; set; }
        public string taskDescription { get; set; }

        public DateTime deadline { set; get; }
        
            
       
        public TaskPriorityModel()
        {
            this.taskName = string.Empty;
            this.taskDescription = string.Empty;
            this.deadline = DateTime.Now;
            this.priority = 1;
        }
        public TaskPriorityModel(string taskName,byte priority):base()
        {
            if (taskName == string.Empty) throw new ArgumentException("Wrong Argument: Task name can't be empty");
            this.taskName = taskName; //ito pizdets kakoito ya ubil 2 chasa iz-za togo 4to ito konstruc  ne dobavil 
            this.deadline = DateTime.Now;
            
            this.taskDescription = string.Empty;
            this.priority = priority;
        }
        public TaskPriorityModel(string taskName,string description, DateTime time ,byte priority) : base()
        {
            if (taskName == string.Empty) throw new ArgumentException("Wrong Argument: Task name can't be empty");
            this.taskName = taskName; //ito pizdets kakoito ya ubil 2 chasa iz-za togo 4to ito konstruc  ne dobavil 
            this.taskDescription = description;
            this.priority = priority;
            this.deadline = time;
        }

        public void setDescription(string description)
        {
            if(description != string.Empty)
            {
                this.taskDescription = description;
            }
        }
    }
}