using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Parsing_CSV_File
{
    class Program
    {
        public class MenuItem
        {
            public int Id { get; set; }
            public string menuName { get; set; }
            public int? parentID { get; set; }
            public bool isHidden { get; set; }
            public string linkURL { get; set; }

            public MenuItem(int Id, string menuName, int? parentId, bool isHidden, string linkUrl)
            {
                this.Id = Id;
                this.menuName = menuName;
                parentID = parentId;
                this.isHidden = isHidden;
                linkURL = linkUrl;

            }
        }
        static void Main(string[] args)
        {
            var path = @"..\..\..\..\Navigation.csv";
            using (StreamReader sr = new StreamReader(path))
            {
                List<MenuItem> menuItems = new List<MenuItem>();
                int index = 0;
                while (!sr.EndOfStream)
                {
                    var csvMenu = sr.ReadLine();                   
                    if (index != 0)
                    {
                        var item = csvMenu.Split(';');
                        var id = int.Parse(item[0]);
                        var menuName = item[1];
                        var parentID = ConvertToNull(item[2]);
                        var isHidden = Convert.ToBoolean(item[3]);
                        var linkURL = item[4];
                        var menuItem = new MenuItem(id, menuName, parentID, isHidden, linkURL);
                        //{
                        //    Id = id,
                        //    menuName = menuName,
                        //    parentID = parentID,
                        //    isHidden = isHidden,
                        //    linkURL = linkURL
                        //};
                        menuItems.Add(menuItem);
                        Console.WriteLine(csvMenu);                        
                    }
                    index++;
                }
                Console.WriteLine("---------------------------------------");
                NavigationMenu(menuItems);                
            }
            Console.ReadLine();
        }
        public static int? ConvertToNull(string v)
        {
            if (int.TryParse(v, out int i)) return i;
            return null;
        }
        public static string GetDots(int level)
        {
            string dot = ".";
            if(level > 0)
            {
                string concat = ".";
                for (int i = 0; i < 3*level; i++)
                {                  
                    concat = concat + dot;
                }
                return concat;
            }
            else  return dot;
        }
        public static List<MenuItem> GetChildrenByParentId(List<MenuItem> menuItems, int? parentId)
        {
            //var parentList = new List<MenuItem>();
            //foreach (var item in menuItems)
            //{               
            //    if (item.parentID == parentId)
            //    {
            //        parentList.Add(item);
            //    }
            //}
            return menuItems.Where(item => item.parentID == parentId).OrderBy(n => n.menuName).ToList(); 
        }
        public static void NavigationMenu(List<MenuItem> menuItems, int? parentId = null , int level = 0)
        {
            List<MenuItem> tempList = GetChildrenByParentId(menuItems, parentId);            
            if(tempList.Count > 0)
            {
                foreach (MenuItem item in tempList)
                {
                    if (item.isHidden == true) continue;
                    string menuItemOutput = $"{GetDots(level)}  {item.menuName}";
                    Console.WriteLine(menuItemOutput);
                    NavigationMenu(menuItems, item.Id, level +1);                   
                }
            }
        }
    }
}
