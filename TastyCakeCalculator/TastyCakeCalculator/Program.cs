using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TastyCakeCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<CakeMould> tastyCakes = BuildAllTastyCakes();

            Console.Out.WriteLine("There are {0} different tasty cakes.", tastyCakes.Count());
            Console.Out.WriteLine("Press a key to close");
            Console.In.ReadLine();
        }

        private static IEnumerable<CakeMould> BuildAllTastyCakes()
        {
            // Primero creamos todas las tortas que tienen la primer fila "tasty"
            IEnumerable<CakeMould> firstRowTastyCakes = BuildFirstRowTastyCakes();

            IEnumerable<CakeMould> tastyCakes = firstRowTastyCakes
                // Ahora vamos rotando las filas de modo de generar el resto de las tortas "tasty" horizontales
                .Union(RearrangeRows(firstRowTastyCakes, 0, 2, 1))
                .Union(RearrangeRows(firstRowTastyCakes, 1, 0, 2))
                .Union(RearrangeRows(firstRowTastyCakes, 1, 2, 0))
                .Union(RearrangeRows(firstRowTastyCakes, 2, 0, 1))
                .Union(RearrangeRows(firstRowTastyCakes, 2, 1, 0));

            // Y ahora trasponemos las tortas existentes, y generan las tortas ricas verticales
            return tastyCakes.Union(Traspose(tastyCakes));
        }

        private static IEnumerable<CakeMould> BuildFirstRowTastyCakes()
        {
            return BuildFirstRowTastyCakes(Ingredient.Confite).Union(BuildFirstRowTastyCakes(Ingredient.Dulce)).Union(BuildFirstRowTastyCakes(Ingredient.Fruta));
        }

        private static IEnumerable<CakeMould> BuildFirstRowTastyCakes(Ingredient ingredient)
        {
            CakeMould c1 = new CakeMould(ingredient, ingredient, ingredient);
            CakeMould c2 = new CakeMould(Ingredient.Masita, ingredient, ingredient);
            CakeMould c3 = new CakeMould(ingredient, Ingredient.Masita, ingredient);
            CakeMould c4 = new CakeMould(ingredient, ingredient, Ingredient.Masita);

            return c1.BuildAllLeft().Union(c2.BuildAllLeft()).Union(c3.BuildAllLeft()).Union(c4.BuildAllLeft());
        }

        private static IEnumerable<CakeMould> RearrangeRows(IEnumerable<CakeMould> tastyCakes, int newFirstRow, int newSecondRow, int newThirdRow)
        {
            return tastyCakes.Select(t => new CakeMould(t, newFirstRow, newSecondRow, newThirdRow));
        }

        private static IEnumerable<CakeMould> Traspose(IEnumerable<CakeMould> tastyCakes)
        {
            return tastyCakes.Select(t => { CakeMould tt = new CakeMould(t); tt.Traspose(); return tt; });
        }
    }
}