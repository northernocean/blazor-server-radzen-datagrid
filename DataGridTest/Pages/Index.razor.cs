using DataGridTest.Data;
using Radzen.Blazor;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DataGridTest.Pages
{
    public partial class Index
    {



        private RadzenGrid<Ship> DataGrid;
        private IEnumerable<Ship> ships;
        private IList<Ship> DDL_Data;
        private readonly IList<string> Messages = new List<string>();
        private string console_output = "";

        protected override void OnInitialized()
        {
            var data_store = new List<Ship>
            {
                new Ship { Id = 1, Name = "Cutty Sark", Launched = 1869 },
                new Ship { Id = 2, Name = "Edmund Fitzgerald", Launched = 1958 },
                new Ship { Id = 3, Name = "Kon Tiki", Launched = 1947 }
            };
            ships = data_store;

            DDL_Data = new List<Ship>
            {
                new Ship { Id = 1, Name = "Cutty Sark", Launched = 1869 },
                new Ship { Id = 2, Name = "Edmund Fitzgerald", Launched = 1958 },
                new Ship { Id = 3, Name = "Kon Tiki", Launched = 1947 },
                new Ship { Id = 1, Name = "Mary Celeste", Launched = 1860 },
                new Ship { Id = 2, Name = "Pequod", Launched = 1840 },
                new Ship { Id = 3, Name = "Queen Anne's Revenge", Launched = 1710 },
                new Ship { Id = 4, Name = "RMS Titanic", Launched = 1911 },
                new Ship { Id = 5, Name = "Trieste", Launched = 1953 }
            };


        }

        protected void Refresh()
        {
            Debug.WriteLine("~~Refresh~~");

        }

        protected void InsertRow()
        {
            print("InsertRow-Before");
            var newShip = new Ship();
            DataGrid.InsertRow(newShip).Wait();
            // await DataGrid.UpdateRow(newRecipe);
            //Save(newShip);
            //Foo(true);
            print("InsertRow-After");

        }

        void OnCreateRow(Ship ship) { print(); }

        void OnUpdateRow(Ship ship) { print(); }

        void OnEditRow(Ship ship) { print(withCounts: false); }

        void Save(Ship ship)
        {
            print("beginning save...", false);
            print();
            DataGrid.UpdateRow(ship).Wait();
            //ships = ships.Append(ship);
            print();

        }

        //DEBUG
        private void MockWriteToConsole(string msg)
        {
            Messages.Add(msg);
            while (Messages.Count() > 30)
            {
                Messages.RemoveAt(0);
            }
            console_output = string.Join("\n", Messages);
            StateHasChanged();
        }

        private void print(
            [System.Runtime.CompilerServices.CallerMemberName] string name = "", 
            bool withCounts = true)
        {
            string msg = name;
            if (withCounts)
                msg += $"({ships.Count()},{DataGrid.Data.Count()})";
            MockWriteToConsole(msg);
            Debug.WriteLine(msg);
        }

        //DEBUG
        protected void Foo(bool detailed = false)
        {
            print();
            if (detailed)
            {
                Debug.WriteLine("ship data---------");
                foreach (var item in ships)
                {
                    Debug.WriteLine(item.Name + " " + item.Launched);
                }
                Debug.WriteLine("grid data---------");
                foreach (var item in ships)
                {
                    Debug.WriteLine(item.Name + " " + item.Launched);
                }
            }
        }

    }


}
