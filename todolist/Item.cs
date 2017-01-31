using System;
using System.Diagnostics;

namespace todolist
{
    public class Item
    {
        [SQLite.Net.Attributes.PrimaryKey, SQLite.Net.Attributes.AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public string timer { get; set; }
        public string date { get; set; }
        public string content { get; set; }
        public bool isChecked { get; set; }
        public string fullDateDisplay
        {
            get
            {
                if (date != null && timer != null)
                {
                    return this.date.Split(' ')[0] + " " + this.timer;
                }
                return "";
            }
        }
        public bool isDone
        {
            get
            {
                if (date != null)
                {
                    string cutDate = date.Split(' ')[0];
                    DateTime itemDate = DateTime.Parse(cutDate);
                    DateTime actualDate = DateTime.Now;
                    if (itemDate.Date > actualDate.Date)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public string isDoneString
        {
            get
            {
                return (isDone ? "is done" : "not done");
            }
        }
        public Item()
        {}
        public Item(string Name,
                    int Id = -1,
                    string Date = null,
                    string Time = null,
                    string Content = null)
        {
            this.id = Id;
            this.name = Name;
            this.timer = Time;
            this.date = Date;
            this.content = Content;
            this.isChecked = false;
        }

    }
}
