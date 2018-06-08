using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chotaiko.Game
{
    class ChotaikoKey
    {
        private int CurrentNoteID;
        private bool IsHolding;
        private int ID;

        public ChotaikoKey()
        {
            this.IsHolding = false;
            this.CurrentNoteID = -1;
        }

        bool IsKey(int ID)
        {
            return this.ID == ID;
        }

        void OnPress()
        {
            this.CurrentNoteID = ChotaikoGlobalContext.OnPress(this);
            this.IsHolding = true;
        }

        void OnRelease()
        {
            this.CurrentNoteID = -1;
            this.IsHolding = false;
        }

        public int GetCurrentNoteID()
        {
            return this.CurrentNoteID;
        }

        public bool GetIsHolding()
        {
            return this.IsHolding;
        }
    }
}
