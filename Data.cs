using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Trello_Desktop
{
    [Serializable]
    class Project
    {
        private List<string> json = new List<string>();

        public DateTime? Current = null;
        public DateTime? last = null;
        public String Name { get; set; }
        public String URL { get; set; }
        public List<string> Json { get { return this.json; } }
        public void getFromJson()
        {
            if (Json.Count == 0)
                return;
            var json = JObject.Parse(Json[0]);

            Current = DateTime.Parse(json["dateLastView"].ToString().Substring(1, json["dateLastView"].ToString().IndexOf(".") - 1));
          
            var list = json["lists"];
            foreach(var t  in list)
            {
                var s = new Categories();
                s.Id = t["id"].ToString();
                s.Name = t["name"].ToString();
                Categories.Add(s);
            }
             list = json["cards"];
            foreach (var t in list)
            {
                var s = new Cards();
                s.Id = t["id"].ToString();
                s.Datetime = DateTime.Parse(t["dateLastActivity"].ToString().Substring(1, t["dateLastActivity"].ToString().IndexOf(".") - 1));
          
                s.Name = t["name"].ToString();
                s.Description = t["desc"].ToString();
               var idList = t["idList"].ToString();
              var cat =  Categories.Where(w => w.Id == idList).First();
                cat.Cards.Add(s);
            }
            list = json["actions"];
            foreach (var t in list)
                if (t["data"]["text"] != null)
            {
                var s = new Comment();
                s.Datetime = DateTime.Parse(t["date"].ToString().Substring(1, t["date"].ToString().IndexOf(".") - 1));
                s.Text = t["data"]["text"].ToString();

                var idList = t["data"]["card"]["id"].ToString();
                var cat = Categories.Where(w => w.Cards.Where(p => p.Id == idList).Count() > 0).First();
                var card = cat.Cards.Where(p => p.Id == idList).First();
                card.Comments.Add(s);
            }
            list = json["checklists"];
            foreach (var t in list)
            {
                var s = new CheckList();
                s.Id = t["id"].ToString();
                s.Name = t["name"].ToString();

                var list2 = t["checkItems"];
                foreach(var t2 in list2)
                {
                    var s1 = new CheckListItem();
                    s1.Id = t2["id"].ToString();
                    s1.Name = t2["name"].ToString();
                    s1.Completed = t2["state"].ToString().Replace('"', ' ').Trim() == "complete";
                    s.Items.Add(s1);
                }



                var idList = t["idCard"].ToString();
                var cat = Categories.Where(w => w.Cards.Where(p=> p.Id == idList).Count()>0).First();
                var card = cat.Cards.Where(p => p.Id == idList).First();
                card.CheckList.Add(s);
            }
                
        }
        public void loadFromFile(string Filename)
        {
            using (StreamReader sr = new StreamReader(Filename.Trim()))
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    this.Json.Add(line);

                }
            getFromJson();
        }
        public void loadFromUrl()
        {
         //   HttpWebRequest wr = (HttpWebRequest)WebRequest.Create("https://trello.com/b/RmEP39yV.json");
         //   Stream rs = wr.GetRequestStream();
            WebRequest request = WebRequest.Create("https://trello.com/b/RmEP39yV.json");

            request.Credentials = new NetworkCredential("harryjfk@qbabit.com", "WatashiMonoda*2016");
            WebResponse response = request.GetResponse();
        }

        private List<Categories> categories = new List<Categories>();
        public List<Categories> Categories { get {

            if (categories == null)
                categories = new List<Categories>();
            return  categories; } }

        public DateTime? Date { get { return Current; } } 
    }
    [Serializable]
    class Categories
    {
        private List<Cards> cards = new List<Cards>();
        public String Name { get; set; }
     
        public String Id { get; set; }
        public List<Cards> Cards { get { return cards; } }

    }
    [Serializable]
    class Cards
    {

        public String Name { get; set; }
        public String Description { get; set; }
        public String Id { get; set; }
        public DateTime Datetime { get; set; }

        private List<CheckList> checklists = new List<CheckList>();
        public List<CheckList> CheckList { get { return checklists; } }
        private List<Comment> comments = new List<Comment>();
        public List<Comment> Comments { get { if (comments == null) comments = new List<Comment>(); return comments; } }
    }
    [Serializable]
    class Comment
    {

        public String Text { get; set; }
        public DateTime Datetime { get; set; }
        public String Id { get; set; }

       
    }
    [Serializable]
    class CheckList
    {

        public String Name { get; set; }
        
        public String Id { get; set; }
        
          private List<CheckListItem> items = new List<CheckListItem>();
          public List<CheckListItem> Items { get { return items; } }

    }
    [Serializable]
    class CheckListItem
    {

        public String Name { get; set; }

        public String Id { get; set; }
        public DateTime Datetime { get; set; }
        public bool Completed { get; set; }
     


    }
    [Serializable]
    class Data {
        private List<Project> list = new List<Project>();

        public List<Project> List { get { return this.list; } }
    }
}
