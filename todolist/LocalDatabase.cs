using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace todolist
{
    class LocalDatabase
    {
        public LocalDatabase()
        {
        }
        public void createDb()
        {
            var sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Itemdb.sqlite");

            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
            {
                conn.CreateTable<Item>();
            }
        }
        public void addItem(Item item)
        {
            var sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Itemdb.sqlite");
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
            {
                conn.RunInTransaction(() =>
                {
                    conn.Insert(item);
                });
            }
        }
        public void deleteItem(int id)
        {
            var sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Itemdb.sqlite");
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
            {
                Item item = conn.Query<Item>("select * from Item where id =" + id).FirstOrDefault();
                if (item != null)
                {
                    conn.RunInTransaction(() =>
                    {
                        conn.Delete(item);
                    });
                }
            }
        }
        public Item getItem(int id)
        {
            var sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Itemdb.sqlite");
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
            {
                var item = conn.Query<Item>("select * from Item where id =" + id).FirstOrDefault();
                return item;
            }
        }
        public ObservableCollection<Item> getObservableItem()
        {
            var sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Itemdb.sqlite");
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
            {
                List<Item> tmp = conn.Table<Item>().ToList<Item>();
                ObservableCollection<Item> observableList = new ObservableCollection<Item>(tmp);
                return observableList;
            }

        }
        public List<Item> getListItem()
        {
            var sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Itemdb.sqlite");
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
            {
                List<Item> tmp = conn.Table<Item>().ToList<Item>();
                List<Item> get = new List<Item>(tmp);
                return get;
            }
        }
        public void updateItem(int id, string date, string timer, string content)
        {
            var sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Itemdb.sqlite");
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
            {

                Item item = conn.Query<Item>("select * from Item where id =" + id).FirstOrDefault();
                if (item != null)
                {
                    item.date = date;
                    item.timer = timer;
                    item.content = content;
                    conn.RunInTransaction(() =>
                    {
                        conn.Update(item);
                    });
                }
            }
        }

        public void updateItem(Item item)
        {
            var sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Itemdb.sqlite");
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
            {
                Item newItem = conn.Query<Item>("select * from Item where id =" + item.id).FirstOrDefault();
                if (newItem != null)
                {
                    newItem.date = item.date;
                    newItem.timer = item.timer;
                    newItem.content = item.content;
                    newItem.isChecked = item.isChecked;
                    conn.RunInTransaction(() =>
                    {
                        conn.Update(newItem);
                    });
                }
            }
        }
        public void un_selectAllItem()
        {
            var sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Itemdb.sqlite");
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
            {
                List<Item> tmp = conn.Table<Item>().ToList<Item>();
                bool isSelect = allItemIsSelected();
                foreach (var item in tmp)
                {
                    item.isChecked = !isSelect;
                    conn.RunInTransaction(() =>
                    {
                        conn.Update(item);
                    });
                }
            }
        }
        public bool allItemIsSelected()
        {
            var sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Itemdb.sqlite");
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
            {
                List<Item> tmp = conn.Table<Item>().ToList<Item>();
                foreach (var item in tmp)
                {
                    if (!item.isChecked)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public void deleteAllDone()
        {
            var sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Itemdb.sqlite");
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
            {
                List<Item> tmp = conn.Table<Item>().ToList<Item>();
                foreach (var item in tmp)
                {
                    if (item.isChecked)
                    {
                        conn.RunInTransaction(() =>
                        {
                            conn.Delete(item);
                        });
                    }
                }
            }
        }
        public void deleteAllItem()
        {
            var sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Itemdb.sqlite");
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
            {
                conn.DropTable<Item>();
                conn.CreateTable<Item>();
                conn.Dispose();
                conn.Close();
            }
        }

    }

}
