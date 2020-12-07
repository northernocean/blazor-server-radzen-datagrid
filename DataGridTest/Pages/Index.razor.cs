using DataGridTest.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DataGridTest.Pages
{
    public partial class Index
    {

        [Inject]
        IJSRuntime JS { get; set; }


        private RadzenGrid<Ship> DataGrid;
        private IList<Ship> shipsDB = new List<Ship>();
        private IList<Ship> ships = new List<Ship>();
        private IList<string> Messages = new List<string>();
        private string console_output = "";

        protected override void OnInitialized()
        {
            base.OnInitialized();

            // Representation of persistent "database" data (i.e., external DB)
            PopulateShipsDB();

            // Representation of transient local data (i.e., queried/retrieved from DB)
            foreach(var ship in shipsDB)
            {
                ships.Add(ship.Clone());
            }

        }

        void EditRow(Ship ship)
        {
            // ---------------------------------------
            // Blazor Grid Demo: grid.EditRow(context);

            DataGrid.EditRow(ship);
        }

        void OnUpdateRow(Ship ship)
        {
            // --------------------------------------------------
            // Blazor Grid Demo: update dbContext, save dbContext.
            //  eg. dbContext.Update(order);
            //      dbContext.SaveChanges();
            //  Also for demo purposes only sync local object with dbContext object

            //Question: any reason to do this here rather than in the UpdateRow() method?

            print("OnUpdateRow " + data_counts);
        }

        void SaveRow(Ship ship)
        {
            // -----------------------------------------
            // Blazor Grid Demo: grid.UpdateRow(context);

            print("SaveRow-Start " + data_counts);
            DataGrid.UpdateRow(ship);
            //ships = ships.Append(ship);
            print("SaveRow-End " + data_counts);
        }

        void CancelEdit(Ship ship)
        {
            // ---------------------------------------------
            // Blazor Grid Demo: grid.CancelEditRow(context);
            // if modified, restore original state (uses EF tracking)

            print("CancelEdit-Start " + $"{ship.Id}/{ship.Name}/{ship.Launched}");

            DataGrid.CancelEditRow(ship);

            if(ship.Id > 0)
            {
                //If we were editing an existing record, then restore edited record to unmodified state.
                Ship shipInDB = shipsDB.Where(c => c.Id == ship.Id).SingleOrDefault();
                if(shipInDB != null)
                {
                    ship.Update(shipInDB);
                }
                else
                {
                    print("Error - invalid state encountered: ShipID > 0 but ship was never saved to DB.");
                }
                print("CancelEdit-End " + $"{ship.Id}/{ship.Name}/{ship.Launched}");
            }
        }

        void DeleteRow(Ship ship)
        {
            // ----------------
            // Blazor Grid Demo:
            // IF ship in data_store (local) THEN
            //   remove from data_store (local) -- ex. dbContext.Remove<T>)(t)
            //   persist changes to underlying data_store -- ex. dbContext.SaveChanges()
            //   DataGrid.Reload();
            //ELSE
            //   DataGrid.CancelEditRow(ship);
        }

        protected void InsertRow()
        {
            // -----------------------------------------
            // Blazor Grid Demo: grid.InsertRow(context);

            print("InsertRow-Start " + data_counts);
            var newShip = new Ship();
            DataGrid.InsertRow(newShip);
            //await DataGrid.UpdateRow(newRecipe);
            //Save(newShip);
            //Foo(true);
            print("InsertRow-End " + data_counts);
        }

        void OnCreateRow(Ship ship)
        {
            // ----------------
            // Blazor Grid Demo:
            //   dbContext.Add(order);
            //   dbContext.SaveChanges();
            //   Also for demo purposes only sync local object with dbContext object
            //    eg: order.Customer = dbContext.Customers.Find(order.CustomerID);
            //        order.Employee = dbContext.Employees.Find(order.EmployeeID);
            // Note: This implies saving the row permanently as soon as a row is created.

            print("OnCreateRow " + data_counts);

        }

        void OnEditRow(Ship ship)
        {
            // ---------------------------------------
            // Blazor Grid Demo: no definition in demo

            print("OnEditRow " + data_counts);
        }

        //DEBUG
        private async void MockWriteToConsole(string msg)
        {
            string[] lines = msg.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None);

            Messages = Messages.Concat(lines).ToList();
            while (false && Messages.Count() > 30)
            {
                Messages.RemoveAt(0);
            }
            console_output = string.Join("\n", Messages);
            StateHasChanged();
            await JS.InvokeVoidAsync("setMyScrollTextArea");
        }

        private void print(string msg)
        {
            MockWriteToConsole(msg);
            Debug.WriteLine(msg);
        }

        //DEBUG
        private string Dump()
        {
            string s = "";
            s += "shipsDB data: { ";
            foreach (var item in shipsDB)
            {
                s += $"{item.Id}: {item.Name} ({item.Launched}), ";
            }
            s = s.Substring(0, s.Length - 2);
            s += " }\n";
            s += "  ships data: { ";
            foreach (var item in ships)
            {
                s += $"{item.Id}: {item.Name} ({item.Launched}), ";
            }
            s = s.Substring(0, s.Length - 2);
            s += " }\n";

            s += "DG.data data: { ";
            foreach (var item in DataGrid.Data)
            {
                s += $"{item.Id}: {item.Name} ({item.Launched}), ";
            }
            s = s.Substring(0, s.Length - 2);
            s += " }";
            return s;
        }

        protected void Refresh()
        {
            print(data_counts);
            print(Dump());
        }

        private string data_counts => $"({shipsDB.Count()}, {ships.Count()}, {DataGrid.Data.Count()})";

        private void PopulateShipsDB()
        {
            shipsDB.Add(new Ship { Id=1, Name="Cutty Sark", Launched=1869 });
            shipsDB.Add(new Ship { Id=2, Name="Edmund Fitzgerald", Launched=1958 });
            shipsDB.Add(new Ship { Id=3, Name="Kon Tiki", Launched=1947 });
            shipsDB.Add(new Ship { Id=4, Name="Edmund Fitzgerald", Launched=1958 });
            //shipsDB.Add(new Ship { Id=5, Name="Kon Tiki", Launched=1947 });
            //shipsDB.Add(new Ship { Id=6, Name="Mary Celeste", Launched=1860 });
            //shipsDB.Add(new Ship { Id=7, Name="Pequod", Launched=1840 });
            //shipsDB.Add(new Ship { Id=8, Name="Queen Anne's Revenge", Launched=1710 });
            //shipsDB.Add(new Ship { Id=9, Name="RMS Titanic", Launched=1911 });
            //shipsDB.Add(new Ship { Id=10, Name="Trieste", Launched=1953 });
        }

    }


}
