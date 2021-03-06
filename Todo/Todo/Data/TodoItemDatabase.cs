﻿using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using Todo.Models;
using Xamarin.Forms;
using XamarinForms.SQLite.SQLite;

namespace Todo
{
    public class TodoItemDatabase
    {
        static object locker = new object();

        SQLiteConnection database;

        /// <summary>
        /// Initializes a new instance of the TodoDB.db3 database. 
        /// if the database doesn't exist, it will create the database and all the tables.
        /// </summary>
        /// <param name='path'>
        /// Path.
        /// </param>
        public TodoItemDatabase()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            // create the tables
            database.CreateTable<TodoItem>();
        }

        public IEnumerable<TodoItem> GetItems()
        {
            lock (locker)
            {
                return (from i in database.Table<TodoItem>() select i).ToList();
            }
        }

        public IEnumerable<TodoItem> GetCompletedItems()
        {
            lock (locker)
            {
                return database.Query<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 1");
            }
        }

        public TodoItem GetItem(int id)
        {
            lock (locker)
            {
                return database.Table<TodoItem>().FirstOrDefault(x => x.Id == id);
            }
        }

        public int SaveItem(TodoItem item)
        {
            lock (locker)
            {
                if (item.Id != 0)
                {
                    database.Update(item);
                    return item.Id;
                }
                else
                {
                    return database.Insert(item);
                }
            }
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                return database.Delete<TodoItem>(id);
            }
        }
    }
}