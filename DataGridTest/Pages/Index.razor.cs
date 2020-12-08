using DataGridTest.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DataGridTest.Pages
{
    public partial class Index
    {

        [Inject]
        IJSRuntime JS { get; set; }


        // Primary data structures
        private RadzenGrid<Ship> DataGrid;
        private readonly IList<Ship> shipsDB = new List<Ship>();
        private readonly IList<Ship> ships = new List<Ship>();

        private bool EditMode = false;

        // For Debugging
        private IList<string> ConsoleMessages = new List<string>();
        private string mock_console_output = "";
        private string data_display = "";

        // For Test Data
        private readonly Queue<Ship> TestShips = new Queue<Ship>();
        private readonly Random random = new Random(42);

        protected override void OnInitialized()
        {
            base.OnInitialized();

            // Representation of persistent "database" data (i.e., external DB)
            PopulateShipsDB();

            // A few extra records for testing
            PopulateTestShips();

            // Representation of transient local data (i.e., queried/retrieved from DB)
            foreach (var ship in shipsDB)
            {
                ships.Add(ship.Clone());
            }
        }

        void EditRow(Ship ship)
        {
            // ---------------------------------------
            // Blazor Grid Demo: grid.EditRow(context);
            Status("start");
            EditMode = true;
            DataGrid.EditRow(ship);
            Status("end");
        }

        void OnUpdateRow(Ship ship)
        {
            // --------------------------------------------------
            // Blazor Grid Demo: update dbContext, save dbContext.
            //  eg. dbContext.Update(order);
            //      dbContext.SaveChanges();
            //  Also for demo purposes only sync local object with dbContext object

            //Question: any reason to do this here rather than in the UpdateRow() method?

            Status("OnUpdateRow " + data_counts);
        }

        void SaveRow(Ship ship)
        {
            // -----------------------------------------
            // Blazor Grid Demo: grid.UpdateRow(context);

            Status("start");
            DataGrid.UpdateRow(ship);
            SaveChanges(ship);
            EditMode = false;
            Status("end");
        }

        void CancelEdit(Ship ship)
        {
            // ---------------------------------------------
            // Blazor Grid Demo: grid.CancelEditRow(context);
            // if modified, restore original state (uses EF tracking)

            Status("start");

            DataGrid.CancelEditRow(ship);

            if (ship.Id > 0)
            {
                RestoreModifiedRecordToOriginalState(ship);
            }

            EditMode = false;
            Status("end");
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

            Status("start");
            Status("Delete Not Implemented");
            Status("end");
        }

        protected void InsertRow()
        {
            // -----------------------------------------
            // Blazor Grid Demo: grid.InsertRow(context);

            Status("start");
            DataGrid.InsertRow(GetTestShip());
            //await DataGrid.UpdateRow(newRecipe);
            //Save(newShip);
            //Foo(true);
            EditMode = true;
            Status("end");
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

            Status("");

        }

        void OnEditRow(Ship ship)
        {
            // ---------------------------------------
            // Blazor Grid Demo: no definition in demo

            Status("");
        }

        private async void MockWriteToConsole(string msg)
        {
            string[] lines = msg.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None);

            ConsoleMessages = ConsoleMessages.Concat(lines).ToList();
            while (false && ConsoleMessages.Count() > 16)
            {
                ConsoleMessages.RemoveAt(0);
            }
            mock_console_output = string.Join("\n", ConsoleMessages);
            StateHasChanged();
            await JS.InvokeVoidAsync("setMyScrollTextArea");
        }

        private void Status(string flag, [CallerMemberName] string caller = "")
        {
            string s = caller + " " + data_counts + (flag == "" ? "" : " /* " + flag + " */");
            if (flag.ToLower() == "end")
                s += "\n";
            MockWriteToConsole(s);
            Dump();
            StateHasChanged();
        }

        private void Dump()
        {
            string s = "";
            s += "shipsDB: { ";
            foreach (var item in shipsDB)
            {
                s += $"{item.Id}/{item.Name}/{item.Launched}, ";
            }
            s = s.Substring(0, s.Length - 2);
            s += " }\n";
            s += "  ships: { ";
            foreach (var item in ships)
            {
                s += $"{item.Id}/{item.Name}/{item.Launched}, ";
            }
            s = s.Substring(0, s.Length - 2);
            s += " }\n";

            s += "DG.data: { ";
            foreach (var item in DataGrid.Data)
            {
                s += $"{item.Id}/{item.Name}/{item.Launched}, ";
            }
            s = s.Substring(0, s.Length - 2);
            s += " }";
            data_display = s;
        }

        protected void Refresh()
        {
            Dump();
            StateHasChanged();
        }

        private string data_counts => $"({shipsDB.Count()}, {ships.Count()}, {DataGrid.Data.Count()})";

        private void SaveChanges(Ship ship)
        {
            var max = shipsDB.Select(c => c.Id).Max();
            ship.Id = max + 1;
            shipsDB.Add(ship.Clone());
        }

        private void RestoreModifiedRecordToOriginalState(Ship ship)
        {
            // restore an edited but unsaved record back to its to unmodified state.
            Ship shipInDB = shipsDB.Where(c => c.Id == ship.Id).SingleOrDefault();
            if (shipInDB != null)
            {
                ship.Update(shipInDB);
            }
            else
            {
                Status("Error - invalid state encountered: ShipID > 0 but ship was never saved to DB.");
            }
        }

        private void PopulateShipsDB()
        {
            shipsDB.Add(new Ship { Id = 1, Name = "Cutty Sark", Launched = 1869 });
            shipsDB.Add(new Ship { Id = 2, Name = "Mary Celeste", Launched = 1860 });
            shipsDB.Add(new Ship { Id = 3, Name = "Pequod", Launched = 1840 });
        }

        private Ship GetTestShip()
        {
            if(TestShips.Count() > 0)
            {
                return TestShips.Dequeue();
            }
            else
            {
                return new Ship { Name=TestShipName(), Launched=TestShipLaunchYear() };
            }
        }
        private void PopulateTestShips()
        {
            TestShips.Enqueue(new Ship { Name = "Edmund Fitzgerald", Launched = 1958 });
            TestShips.Enqueue(new Ship { Name = "Kon Tiki", Launched = 1947 });
            TestShips.Enqueue(new Ship { Name = "Queen Anne's Revenge", Launched = 1710 });
            TestShips.Enqueue(new Ship { Name = "RMS Titanic", Launched = 1911 });
            TestShips.Enqueue(new Ship { Name = "Trieste", Launched = 1953 });
        }

        private string TestShipName()
        {
            return "Test" + Convert.ToString(shipsDB.Select(c => c.Id).Max() + 1);
        }

        private int TestShipLaunchYear()
        {
            return random.Next(1950, 2020);
        }


    }


}
