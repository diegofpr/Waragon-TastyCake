using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TastyCakeCalculator
{
    public class CakeMould
    {
        private Ingredient[] mould = new Ingredient[]
            {
                Ingredient.Vacio, Ingredient.Vacio, Ingredient.Vacio , 
                Ingredient.Vacio, Ingredient.Vacio, Ingredient.Vacio , 
                Ingredient.Vacio, Ingredient.Vacio, Ingredient.Vacio , 
            };

        #region Constructors

        public CakeMould(Ingredient firstIngredient, Ingredient secondIngredient, Ingredient thirdIngredient)
        {
            Set(0, 0, firstIngredient);
            Set(0, 1, secondIngredient);
            Set(0, 2, thirdIngredient);
            Rehash();
        }

        public CakeMould(CakeMould x, int newFirstRow = 0, int newSecondRow = 1, int newThirdRow = 2)
        {
            for (int i = 0; i < 3; i++)
            {
                this.Set(0, i, x.Get(newFirstRow, i));
                this.Set(1, i, x.Get(newSecondRow, i));
                this.Set(2, i, x.Get(newThirdRow, i));
            }
        }

        #endregion

        #region Get / Set

        public void Set(int x, int y, Ingredient ingredient)
        {
            if (mould[x * 3 + y] == Ingredient.Vacio)
            {
                mould[x * 3 + y] = ingredient;
                Rehash();
            }
            else
            {
                throw new Exception("No se puede reemplazar un ingrediente.");
            }
        }

        // Se asume que se la llama estando seguro de que hay vacios, sino lanza excepción.
        private void SetFirstEmpty(Ingredient ingredient)
        {
            int i = 0;
            do
            {
                i++;
            }
            while (this.mould[i] != Ingredient.Vacio);

            this.mould[i] = ingredient;
            Rehash();
        }

        public Ingredient Get(int x, int y)
        {
            return mould[x * 3 + y];
        }
        
        #endregion

        #region IEquality

        public static bool Equals(CakeMould cakeMould1, CakeMould cakeMould2)
        {
            for (int i = 0; i < 9; i++)
            {
                if (cakeMould1.mould[i] != cakeMould2.mould[i])
                {
                    return false;
                }
            }
            return true;
        }

        #region Hash Code Handling

        public override int GetHashCode()
        {
            return this.hash;
        }

        private int hash = 0;
        private void Rehash()
        {
            this.hash = 0;

            for (int i = 0; i < 9; i++)
            {
                this.hash += (int)this.mould[i] * (int)Math.Pow(10, i);
            }
        }

        #endregion

        #endregion

        public IEnumerable<CakeMould> BuildAllLeft()
        {
            if (this.mould.Any(i => i == Ingredient.Vacio))
            {
                return this.BuildAllLeft(Ingredient.Confite)
                    .Union(this.BuildAllLeft(Ingredient.Dulce))
                    .Union(this.BuildAllLeft(Ingredient.Fruta))
                    .Union(this.BuildAllLeft(Ingredient.Masita));
            }
            else
            {
                return new List<CakeMould> { this };
            }
        }

        private IEnumerable<CakeMould> BuildAllLeft(Ingredient ingredient)
        {
            int used = this.mould.Count(i => i == ingredient);
            if (used == 1 && ingredient == Ingredient.Masita)
            {
                // No se pueden agregar más de este tipo, 
                // lo cual significa que no se pueden agregar
                // más tortas por ese camino.
                return new List<CakeMould>();
            }
            else if (used == 3 && ingredient != Ingredient.Masita)
            {
                // No se pueden agregar más de este tipo, 
                // lo cual significa que no se pueden agregar
                // más tortas por ese camino.
                return new List<CakeMould>();
            }
            else
            {
                CakeMould c = new CakeMould(this);
                c.SetFirstEmpty(ingredient);
                return c.BuildAllLeft();
            }
        }

        public void Traspose()
        {
            this.Swap(2, 4);
            this.Swap(3, 7);
            this.Swap(6, 8);
        }

        private void Swap(int p1, int p2)
        {
            Ingredient temp = this.mould[p1];
            this.mould[p1] = this.mould[p2];
            this.mould[p2] = temp;
        }

        public bool HasEmptyPlaces { get; set; }
    }
}