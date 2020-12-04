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
        private IEnumerable<Ship> hobbits;
        private IList<Ship> DDL_Data;
        private IList<string> Messages = new List<string>();
        private string console = "";

        protected override void OnInitialized()
        {
            var data = new List<Ship>();
            data.Add(new Ship { Id = 1, Name = "Cutty Sark", Launched = 1869 } );
            data.Add(new Ship { Id = 2, Name = "Edmund Fitzgerald", Launched = 1958 } );
            data.Add(new Ship { Id = 3, Name = "Kon Tiki", Launched = 1947 } );
            hobbits = data;

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
            var newRecipe = new Ship();
            DataGrid.InsertRow(newRecipe).Wait();
            // await DataGrid.UpdateRow(newRecipe);
            Save(newRecipe);
            //Foo(true);
            print("InsertRow-After");

        }

        void OnCreateRow(Ship hobbit) { print(); }

        void OnUpdateRow(Ship hobbit) { print(); }

        void OnEditRow(Ship hobbit) { print(withCounts: false); }

        void Save(Ship hobbit)
        {
            print("beginning save...", false, false);
            print();
            DataGrid.UpdateRow(hobbit).Wait();
            //hobbits = hobbits.Append(hobbit);
            print();

        }

        //DEBUG
        private void MockWriteToConsole(string msg)
        {
            Messages.Add(msg);
            while (Messages.Count() > 20)
            {
                Messages.RemoveAt(0);
            }
            console = string.Join("\n", Messages);
            StateHasChanged();
        }

        private string MockReadFromConsole()
        {
            return string.Join('\n', Messages);
        }

        private void print([System.Runtime.CompilerServices.CallerMemberName] string name = "", bool withCounts = true, bool withSquiggles = true)
        {
            string msg = "~~" + name;
            if (withCounts)
                msg += $"({hobbits.Count()},{DataGrid.Data.Count()})";
            msg += "~~";
            Debug.WriteLine(msg);
        }

        //DEBUG
        protected void Foo(bool detailed = false)
        {
            print();
            if (detailed)
            {
                Debug.WriteLine("hobbit data---------");
                foreach (var item in hobbits)
                {
                    Debug.WriteLine(item.Name + " " + item.Launched);
                }
                Debug.WriteLine("grid data---------");
                foreach (var item in hobbits)
                {
                    Debug.WriteLine(item.Name + " " + item.Launched);
                }
            }
        }

    }


}
